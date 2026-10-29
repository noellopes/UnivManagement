using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers {
    // Files are uploaded, downloaded and deleted from the details page of a request
    public class ServiceRequestAttachmentsController : Controller {
        private readonly UniversityDbContext _context;
        private readonly FileExtensionContentTypeProvider _contentTypes = new();

        public ServiceRequestAttachmentsController(UniversityDbContext context) {
            _context = context;
        }

        // POST: ServiceRequestAttachments/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(ServiceRequestAttachment.MaxSize + 1024 * 1024)]
        public async Task<IActionResult> Upload(int serviceRequestId, IFormFile? file) {
            var serviceRequest = await _context.ServiceRequests.FindAsync(serviceRequestId);
            if (serviceRequest == null) {
                return NotFound();
            }

            string? error = ValidateFile(file, serviceRequest);
            if (error != null) {
                TempData["Error"] = error;
                return RedirectToDetails(serviceRequestId);
            }

            using var stream = new MemoryStream();
            await file!.CopyToAsync(stream);

            // The file type comes from the extension, not from what the browser sends
            string fileName = Path.GetFileName(file.FileName);
            if (!_contentTypes.TryGetContentType(fileName, out string? contentType)) {
                contentType = "application/octet-stream";
            }

            var attachment = new ServiceRequestAttachment {
                ServiceRequestId = serviceRequestId,
                FileName = fileName,
                ContentType = contentType,
                Size = file.Length,
                Content = stream.ToArray(),
                UploadedAt = DateTime.Now
            };
            _context.ServiceRequestAttachments.Add(attachment);

            serviceRequest.History.Add(new ServiceRequestHistory { Action = $"File {attachment.FileName} attached" });
            serviceRequest.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["Message"] = $"The file {attachment.FileName} was attached.";
            return RedirectToDetails(serviceRequestId);
        }

        // GET: ServiceRequestAttachments/Download/5
        public async Task<IActionResult> Download(int id) {
            var attachment = await _context.ServiceRequestAttachments
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.ServiceRequestAttachmentId == id);
            if (attachment == null) {
                return NotFound();
            }

            return File(attachment.Content, attachment.ContentType, attachment.FileName);
        }

        // POST: ServiceRequestAttachments/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id) {
            var attachment = await _context.ServiceRequestAttachments
                .Include(a => a.ServiceRequest)
                .FirstOrDefaultAsync(a => a.ServiceRequestAttachmentId == id);
            if (attachment == null) {
                return NotFound();
            }

            if (attachment.ServiceRequest!.IsClosed) {
                TempData["Error"] = "Files of a closed request cannot be deleted.";
                return RedirectToDetails(attachment.ServiceRequestId);
            }

            attachment.ServiceRequest.History.Add(new ServiceRequestHistory { Action = $"File {attachment.FileName} removed" });
            attachment.ServiceRequest.UpdatedAt = DateTime.Now;
            _context.ServiceRequestAttachments.Remove(attachment);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"The file {attachment.FileName} was removed.";
            return RedirectToDetails(attachment.ServiceRequestId);
        }

        // Returns the reason why the file cannot be attached, or null if it is valid
        private static string? ValidateFile(IFormFile? file, ServiceRequest serviceRequest) {
            if (serviceRequest.IsClosed) {
                return "Files cannot be attached to a closed request.";
            }
            if (file == null || file.Length == 0) {
                return "Choose a file to attach.";
            }
            if (file.Length > ServiceRequestAttachment.MaxSize) {
                return $"The file cannot be larger than {ServiceRequestAttachment.MaxSize / (1024 * 1024)} MB.";
            }
            if (Path.GetFileName(file.FileName).Length > 255) {
                return "The file name cannot exceed 255 characters.";
            }
            if (!ServiceRequestAttachment.IsAllowedExtension(file.FileName)) {
                return "Only these file types are allowed: " + string.Join(", ", ServiceRequestAttachment.AllowedExtensions) + ".";
            }
            return null;
        }

        private IActionResult RedirectToDetails(int serviceRequestId) {
            return RedirectToAction("Details", "ServiceRequests", new { id = serviceRequestId }, "attachments");
        }
    }
}
