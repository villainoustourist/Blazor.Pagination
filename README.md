# Blazor.Pagination
A reusable pagination component for .NET Core Blazor.

[![Build Status](https://kfranken.visualstudio.com/Personal/_apis/build/status/Build%20%26%20Publish%20Blazor%20Pagination?branchName=master)](https://kfranken.visualstudio.com/Personal/_build/latest?definitionId=13&branchName=master)
[![Package Version](https://img.shields.io/nuget/v/BlazorPagination.svg)](https://www.nuget.org/packages/BlazorPagination/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/BlazorPagination.svg)](https://www.nuget.org/packages/BlazorPagination/)
[![License](https://img.shields.io/github/license/villainoustourist/Blazor.Pagination.svg)](https://github.com/villainoustourist/Blazor.Pagination/blob/master/LICENSE)


## Installation

Add the BlazorPagination NuGet package to your .NET Core Blazor app using the following command in the Package Manager Console:
```
PM> Install-Package BlazorPagination
```
or using .NET CLI run the following command
```
dotnet add package BlazorPagination
```

Once you have installed the package add the following line in the ```_ViewImports.cshtml``` file
```
@addTagHelper *,BlazorPagination
```
## Usage

```
...
@using BlazorPagination
...
<table class="table">
    <tbody>
    @foreach (var item in _data.Results)
    {
    <tr>@item</tr>
    }
    </tbody>
</table>
<BlazorPager CurrentPage="@_data.CurrentPage"
         PageCount="@_data.PageCount"
         OnPageChanged="(async e => { _page = e; await FetchData();})"

         ShowFirstLast="false"
         ShowPageNumbers="true"
         VisiblePages="10"
         FirstText="First"
         LastText="Last" />
...
@code {
    private PagedResult<string> _data;
    private string _filter;
    private int _page = 1;
    protected override OnInitialized()
    {
        _data = FetchData(_filter, _page);
    }
}
```

Default HTML output:

```
<ul class="pagination justify-content-end"> 
    <li class="page-item disabled"><a class="page-link"><span aria-hidden="true">Previous</span><span class="sr-only">Go to previous page</span></a></li>
    <li class="page-item active"><a class="page-link">1</a></li>
    <li class="page-item"><a class="page-link btn btn-link">2</a></li>
    <li class="page-item"><a class="page-link btn btn-link">3</a></li>
    <li class="page-item"><a class="page-link btn btn-link">4</a></li>
    <li class="page-item"><a class="page-link btn btn-link">5</a></li>
    <li class="page-item"><a class="page-link btn btn-link"><span aria-hidden="true">Next</span><span class="sr-only">Go to next page</span></a></li>
</ul>
```
