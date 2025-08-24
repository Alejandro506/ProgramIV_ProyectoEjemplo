# Products Per Category - Master-Detail with AJAX

## Overview
This feature demonstrates a master-detail view implementation using AJAX for dynamic content loading without full page refreshes.

## Features
- **Master Section**: Dropdown with ProductCategory.Name values
- **Detail Section**: Table showing products with ProductID, Name, ListPrice, and Color
- **AJAX Functionality**: Detail section refreshes automatically when master selection changes
- **Loading Indicators**: Visual feedback during data loading
- **Error Handling**: Graceful error handling for failed requests

## Files Created
1. **Controller**: `Controllers/ProductsPerCategoryController.cs`
   - `Index()`: Main action that loads categories and displays the view
   - `GetProductsByCategory(int categoryId)`: AJAX endpoint returning partial view with products

2. **Views**: 
   - `Views/ProductsPerCategory/Index.cshtml`: Main view with master-detail layout
   - `Views/ProductsPerCategory/_ProductsPartial.cshtml`: Partial view for products table

3. **Navigation**: Updated `Views/Shared/_Layout.cshtml` to include menu link

## Technical Implementation
- Uses Entity Framework with the existing AdventureWorksContext
- AJAX calls return partial views for seamless DOM updates  
- Responsive Bootstrap design with cards and styled tables
- Color display with visual badges for better UX
- Currency formatting for prices

## Usage
1. Navigate to "Products Per Category" from the main menu
2. Select a category from the dropdown
3. Watch the product list update automatically via AJAX
4. Visual loading indicator provides feedback during data retrieval

## AJAX Flow
1. User selects category → JavaScript event triggered
2. AJAX request sent to `GetProductsByCategory` action
3. Controller queries database and returns partial view
4. JavaScript updates DOM with new content
5. Loading indicators manage user experience

This implementation demonstrates proper separation of concerns while keeping data access in the controller for simplicity as requested.
