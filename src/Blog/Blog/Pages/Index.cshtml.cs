using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Blog.Pages
{
   public class IndexModel : PageModel
   {
      private readonly ILogger<IndexModel> _logger;
      private readonly MySettings _settings;

      public IndexModel(ILogger<IndexModel> logger, IOptions<MySettings> settingsOptions)
      {
         _logger = logger;
         _settings = settingsOptions.Value;
      }

      public string Message => _settings.Message;

      public void OnGet()
      {

      }
   }
}
