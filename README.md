# Blazor.Pagination
A reusable pagination component for .NET Core Blazor.


![NuGet](https://img.shields.io/nuget/v/BlazorPagination.svg?style=flat-square&label=nuget)

Add the BlazorPagination NuGet package to your .NET Core Blazor app using the following command in the Package Manager Console:
```
PM> Install-Package BlazorPagination
```

To install ```BlazorPagination``` using .NET CLI run the following command
```
dotnet add package BlazorPagination
```

Once you have installed the package add the following line in the ```_ViewImports.cshtml``` file
```
@addTagHelper *,BlazorPagination
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