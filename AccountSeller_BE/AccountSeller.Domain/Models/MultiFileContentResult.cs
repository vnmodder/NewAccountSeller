using Microsoft.AspNetCore.Mvc;

namespace AccountSeller.Domain.Models
{
    public class MultiFileContentResult : ActionResult
    {
        private readonly IEnumerable<FileContentResult> _files;

        public MultiFileContentResult(IEnumerable<FileContentResult> files)
        {
            _files = files;
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            foreach (var file in _files)
            {
                await file.ExecuteResultAsync(context);
            }
        }
    }
}
