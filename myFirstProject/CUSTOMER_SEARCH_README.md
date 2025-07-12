# Customer Search MVC Feature

This document describes the Customer Search MVC functionality that has been added to your project.

## Overview

The Customer Search feature provides a web interface to search for customers using the existing CustomersController API. It includes:

1. **Search Form**: A user-friendly form with three fields:
   - Customer ID (optional)
   - Name (optional) 
   - Max Number of Records (required, 1-50, default: 10)

2. **Results Grid**: A responsive table displaying search results with customer information

## Files Created

### View Models
- `ViewModels/CustomerSearchViewModel.cs` - Form input model with validation
- `ViewModels/CustomerSearchResultsViewModel.cs` - Results display model

### Controller
- `Controllers/CustomerSearchController.cs` - MVC controller handling the search form and API integration

### Views
- `Views/CustomerSearch/Index.cshtml` - Search form page
- `Views/CustomerSearch/Results.cshtml` - Results display page

### Configuration
- Updated `Program.cs` to configure HttpClient for API calls
- Updated `Views/Shared/_Layout.cshtml` to add navigation and FontAwesome icons

## How to Use

1. **Start the application**:
   ```bash
   dotnet run
   ```

2. **Navigate to Customer Search**:
   - Go to `https://localhost:5282/CustomerSearch` 
   - Or click "Customer Search" in the navigation menu

3. **Search Options**:
   - **Find all customers**: Leave both ID and Name empty, set max records
   - **Find specific customer**: Enter Customer ID only
   - **Search by name**: Enter name (supports partial matches)
   - **Combined search**: Enter both ID and name for precise results

4. **View Results**:
   - Results are displayed in a responsive grid
   - Shows Customer ID, Name, Email, Phone, Company, and Sales Person
   - Email addresses are clickable (mailto links)
   - Click "New Search" to return to the search form

## API Integration

The MVC controller calls your existing CustomersController API:
- **Endpoint**: `/Customers`
- **Parameters**: 
  - `customerId` (optional)
  - `name` (optional)
  - `maxNumberOfRecords` (required)

## Features

✅ **Form Validation**: Client and server-side validation
✅ **Responsive Design**: Works on desktop and mobile
✅ **Error Handling**: Displays API errors and validation messages
✅ **Search Tips**: Built-in help for users
✅ **Modern UI**: Bootstrap 5 styling with FontAwesome icons
✅ **Clean URLs**: SEO-friendly routing
✅ **Accessibility**: Proper labels and ARIA attributes

## Technical Details

- **Framework**: ASP.NET Core 9.0 MVC
- **UI Framework**: Bootstrap 5
- **Icons**: FontAwesome 6
- **API Communication**: HttpClient with dependency injection
- **Validation**: Data Annotations with client-side validation
- **Responsive**: Mobile-first design

## Customization

To customize the search functionality:

1. **Change validation rules**: Update `CustomerSearchViewModel.cs`
2. **Modify UI**: Edit the Razor views in `Views/CustomerSearch/`
3. **Add fields**: Extend the view models and update the API call
4. **Styling**: Modify CSS classes or add custom styles

## Troubleshooting

- **API not responding**: Check that the CustomersController API is running on the same port
- **Navigation not working**: Ensure the layout file includes the Customer Search menu item
- **Styling issues**: Verify FontAwesome CDN is loading correctly
- **Validation not working**: Check that `_ValidationScriptsPartial` is included

The feature is now ready to use! 🎉
