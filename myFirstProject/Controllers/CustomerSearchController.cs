using Microsoft.AspNetCore.Mvc;
using myFirstProject.MyModels;
using myFirstProject.ViewModels;
using System.Text.Json;

namespace myFirstProject.Controllers;

/// <summary>
/// MVC Controller for customer search functionality
/// </summary>
public class CustomerSearchController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CustomerSearchController> _logger;
    private readonly IConfiguration _configuration;

    public CustomerSearchController(IHttpClientFactory httpClientFactory, ILogger<CustomerSearchController> logger, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _logger = logger;
        _configuration = configuration;
        
        // Set base address dynamically
        var baseUrl = _configuration["BaseUrl"] ?? "https://localhost:5282/";
        _httpClient.BaseAddress = new Uri(baseUrl);
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "CustomerSearchMVC/1.0");
    }

    /// <summary>
    /// GET: Display the customer search form
    /// </summary>
    public IActionResult Index()
    {
        var model = new CustomerSearchViewModel();
        return View(model);
    }

    /// <summary>
    /// POST: Handle search form submission and display results
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Search(CustomerSearchViewModel searchCriteria)
    {
        var resultsModel = new CustomerSearchResultsViewModel
        {
            SearchCriteria = searchCriteria
        };

        if (!ModelState.IsValid)
        {
            resultsModel.ErrorMessage = "Please correct the validation errors and try again.";
            return View("Results", resultsModel);
        }

        try
        {
            // Build the API query parameters
            var queryParams = new List<string>();
            
            if (searchCriteria.CustomerID.HasValue)
            {
                queryParams.Add($"customerId={searchCriteria.CustomerID.Value}");
            }
            
            if (!string.IsNullOrWhiteSpace(searchCriteria.Name))
            {
                queryParams.Add($"name={Uri.EscapeDataString(searchCriteria.Name)}");
            }
            
            queryParams.Add($"maxNumberOfRecords={searchCriteria.MaxNumberOfRecords}");

            var queryString = string.Join("&", queryParams);
            var apiUrl = $"/Customers?{queryString}";

            _logger.LogInformation("Calling API: {ApiUrl}", apiUrl);

            // Call the API
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                
                var customers = JsonSerializer.Deserialize<List<CustomerViewModel>>(jsonContent, options);
                resultsModel.Results = customers ?? new List<CustomerViewModel>();
            }
            else
            {
                resultsModel.ErrorMessage = $"API call failed with status: {response.StatusCode}. {await response.Content.ReadAsStringAsync()}";
                _logger.LogError("API call failed: {StatusCode} - {Content}", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }
        catch (Exception ex)
        {
            resultsModel.ErrorMessage = $"An error occurred while searching: {ex.Message}";
            _logger.LogError(ex, "Error occurred while calling customer API");
        }

        return View("Results", resultsModel);
    }

    /// <summary>
    /// GET: Display search results (also allows direct access to results page)
    /// </summary>
    public IActionResult Results()
    {
        var model = new CustomerSearchResultsViewModel();
        return View(model);
    }
}
