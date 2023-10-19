# Email templates

## How to create a new template and use it

1. Create a new ``html`` file in this folder.
1. If you need to use values, insert ``{{value_name}}`` where needed. ``value_name`` is the name of the variable.

## An example of using templates

```cs

    public class ExampleApiController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public ExampleApiController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet("email")]
        public async Task<IActionResult> TestEmailService()
        {
            await _emailService.SendEmailUseTemplateAsync("email@example.com", "templateName.html", new Dictionary<string, string>
            {
                {"key1", "value1" },
                {"key2", "value2" }
            });
            return Ok();
        }
    }
```