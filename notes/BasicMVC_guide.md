### Project Setup

1. Create new project `ASP.NET Core Web App (MVC)` with project name `BulkyWeb` and solution name `Bulky` (use `Individual User Accounts` for authentication)
2. In `appsettings.json`, edit `Server` and `Database=Bulky` in `ConnectionStrings`

**Only For Scaffolding Database**

1. Install NuGet package: `Microsoft.EntityFramework.SqlServer`
2. In PM console, run `Scaffold-DbContext "Server=<ServerName>;Database=<DbName>;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models`

### Category CRUD Operations

##### Create First Model (Category Model)

1. Create new class `Models/Category.cs` with props
   - Id: `int`
   - Name: `string` with annotation `Required`
   - DisplayOrder: `int`

##### Database Migration

1. In `Data/ApplicationDbContext.cs` add prop
   - Categories: `DbSet<Category>`
2. In PM console, run
   - `add-migration "AddCategoryTableToDb"`
   - `update-database`

##### Category Controller

1. Create new controller `Controllers/CategoryController.cs`
2. Add View for `Index` Action
3. In NavBar of `_Layout.cshtml`, change `About` to `Category` (`asp-controller="Category"`)

##### Data Seeding

1. In `ApplicationDbContext.cs`, add the following lines to seed data:

```cs
protected override void OnModelCreating(ModelBuilder builder)
{
  base.OnModelCreating(builder);
  builder.Entity<Category>().HasData (
    new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
    new Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
    new Category { Id = 3, Name = "History", DisplayOrder = 3 }
  );
}
```

2. Add migration and update database

##### Get All Categories

1. Create constructor for `CategoryController` which takes `ApplicationDbContext` as the param
2. In Action `CategoryController.Index`, instantiate `List<Category> categories` using `DbContext.Categories.ToList()`
3. Pass `categories` to `View` and add model to `Index.cshtml`
4. Create two columns table (classes: `table`, `table-bordered`, `table-striped`) in `Index.cshtml` displaying `Name` and `DisplayOrder` (use `foreach` to display all categories with `Model.OrderBy(m => m.DisplayOrder)`)

##### Bootswatch And Bootstrap (Optional)

###### Bootswatch (Optional)

1. Go `Bootswatch` and download LUX theme
2. Check `_Layout.cshtml` and see which css file is used
3. Copy and paste the new css file to the old css file
4. Change `navbar` class from `navbar-light` to `navbar-dark` and from `bg-white` to `bg-primary`
5. Remove `text-dark` from all `nav-item`
6. Add class `bg-primary` to footer and change the text to `BulkyWeb`

###### Bootstrap Icon (Optional)

1. Google "Bootstrap icon cdn", copy the cdn and paste in the `head` of `_Layout.cshtml`
2. Get icon `heart`, copy the HTML element and paste next to `BulkyWeb` in the footer

##### Design Category List Page

1. Above the table, create a row container which displays "Category List" on the left and a button (classes: `btn`, `btn-primary`) with label "<plus_icon_from_bootstrap> Create New Category"

```html
<div class="container">
  <div class="row pt-4 pb-3">
    <div class="col-6">
      <h2 class="text-primary">Category List</h2>
    </div>
    <div class="col-6 text-end">
      <a asp-controller="" asp-action="" class="btn btn-primary">
        <i class="bi bi-plus-circle"></i> Create New Category
      </a>
    </div>
  </div>
</div>
```

##### Category Create View

1. Create new Action `CategoryController.Create` and add new View displaying "CREATE CATEGORY"
2. In `Index.cshtml`, set `asp-controller="Category"` and `asp-action="Create"` for the "Create New Category" button (redirect to `Create` View when click)
3. In `Create.cshtml`, add
   - Form with `method=post`
   - Button to submit the POST request
   - Button to redirect back to `Index`

```html
<form method="post">
  <div class="border p-3 mt-4">
    <div class="row pb-2">
      <h2 class="text-primary">Create Category</h2>
      <hr />
    </div>
    <div class="mb-3">
      <label>Category Name</label>
      <input type="text" class="form-control" />
    </div>
    <div class="mb-3">
      <label>Display Order</label>
      <input type="text" class="form-control" />
    </div>
    <div class="row">
      <div class="col-6">
        <button type="submit" class="btn btn-primary form-control">
          Create
        </button>
      </div>
      <div class="col-6">
        <a
          asp-controller="Category"
          asp-action="Index"
          class="btn btn-outline-secondary border form-control"
          >Back to List</a
        >
      </div>
    </div>
  </div>
</form>
```

###### Field Binding

1. In the text input of "Category Name", add `asp-for="Name"` (`Name` maps to the prop of `@model`) and remove `type="text"` (`type` is defined automatically based on the model)
2. In the text input of "Display Order", add `asp-for="DisplayOrder"` and remove `type="text"`
3. In the label of "Category Name", add `asp-for="Name"` and remove the text "Category Name" (text "Name" will be added automatically)
4. In the label of "Display Order", add `asp-for="DisplayOrder"` and remove the text "Display Order" (text "DisplayOrder" will be added automatically)
5. Add annotations for `Name` and `DisplayOrder` in `Category.cs` (eg. `[DisplayName("Category Name")]`)

##### Create Category

1. In `CategoryController`, create another `Create` action (with annotation `[HttpPost]` and input param `Category`)
2. In the `HttpPost` action `Create`

   - Run `DbContext.Categories.Add(category)` to add the Category object (input param) into the database
   - Run `DbContext.SaveChanges()`
   - Return `RedirectToAction("Index","Category")` (the second arg `Category` is optional if redirect within the same controller)

##### Server Side Validation (C# code)

###### Constraints In Model Class

1. Add annotation in `Category.cs`
   - `DisplayOrder`: `[Range(1,100)]`
   - `Name`: `[MaxLength(30)]`
2. In HttpPost `CategoryController.Create(Category obj)`, wrap everything inside `if (ModelState.IsValid)` (Only save if the model object fulfills all constraints)
3. Return `View()` at the end (will only return `View` if validation is failed)
4. In `Create.cshtml`, display error message if validation is failed by adding `<span asp-validation-for="<prop>" class="text-danger"></span>` for both `Name` and `DisplayOrder`
5. Add `ErrorMessage="Display Order must be between 1-100"` as 3rd arg to `Range` annotation in `Category.cs`

###### Customize Validation In Action Method

1. To customize validation rule, add conditions in Action method (eg. `Name` cannot be `Invalid`):

```cs
if (obj.Name == "Invalid") {
  ModelState.AddModelError("Name", "Category name cannot be 'Invalid'!")
}
```

###### Display Validation Summary

1. To display all validation conditions in `Create` View, add `<div asp-validation-summary="All"></div>`

##### Client Side Validation (JS code)

1. In `Create.cshtml`, add JS script at the buttom

```cs
@section Scripts {
  @{
    <partial name="_ValidationScriptsPartial"/>
  }
}
```

##### Edit Category

1. In `Index.cshtml` table, create another column for edit button (use `<a>` tags inside `<div>`)

```html
<div class="w-75 btn-group" role="group">
  <a asp-controller="Category" asp-action="Edit" asp-route-id="@category.Id" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"> Edit</a>
</div>
```

###### Edit (HttpGet)

1. Replicate `Create` (`HttpGet`) in `CategoryController` and rename with `Edit`
2. In HttpGet Action `Edit`, set input param as `int? id`. Inside the Action method:

   - If `id == null || id == 0`, return `NotFound`
   - Else find `category` by Id and pass to `View` if `category` is not null (use `DbContext.Categories.Find(id)` or `FirstOrDefault`)

3. Create `Edit` View (optional to use Razor View), copy everything in `Create.cshtml` and paste to `Edit.cshtml`, change `h2` text to "Edit Category" and `button` text to "Update"
   _Concept_:
   - `asp-for` in `Edit.cshtml` will autofill the input when we pass `Category` to `View`

###### Edit (HttpPost)

1. Replicate `Create` (`HttpPost`) in `CategoryController` and rename with `Edit` (change from `DbContext.Categories.Add` to `DbContext.Categories.Update`)
2. (Optional) In `Edit.cshtml`, add `<input asp-for="Id" hidden />` at the beginning of the form
   _Concept_:
   - It is optional only if the field is `Id`. If the field is other, like `CategoryId`, we need the above line to pre-fill the Id with the original value

##### Delete Category

1. In `Index.cshtml` table, create delete button in the last column
2. Replicate `Edit` (`HttpGet`) in `CategoryController` and rename with `Delete(int? id)`
3. Replicate `Edit` (`HttpPost`) in `CategoryController` and rename with `DeletePost(int? id)` (cannot have the same name as the GET action of delete)
4. Add annotation for `DeletePost`: `[HttpPost, ActionName("Delete")]`
5. In the action `DeletePost`
   - Find `category` by Id
   - If `category` is null, return `NotFound`
   - Else remove the category from DB (`DbContext.Categories.Remove(category)`)
   - Save database change and redirect to `Index`
6. Create view for `Delete` action using `Edit.cshtml`
   - Add `<input asp-for="Id" hidden />` at the beginning of the form
   - Change `h2` text to "Delete Category"
   - Do not need validation
   - Add `disable` inside `input` tag
   - Change `button` text to "Delete" and class from `btn-primary` to `btn-danger`

##### TempData

1. Add `TempData["success"]=<message>` to all HttpGet `Create`, `Edit`, `Delete`
2. In `Index.cshtml` top (after defined `@model`), add

```cs
@if (TempData["success"] != null) {
  <h2>@TempData["success"]</h2>
}
```

##### Partial View

1. Create partial view `Shared/_Notification.cshtml` and cut the lines above from `Index.cshtml` to it
2. In `Index.cshtml`, at the cut area, add `<partial name="_Notification"/>`

##### AJAX On Category List Page

1. Create GET Action `CategoryController.ListAndCreate()`
2. Create View for `ListAndCreate`, copy `Index.cshtml` to `ListAndCreate.cshtml`
3. In `_Layout.cshtml`, add another `li` to navbar to navigate to `ListAndCreate`
4. Remove the `Create New Category` button in `ListAndCreate.cshtml`
5. Create inputs for "Category Name" and "Display Order", and "Create Category" button (can copy from `Create.cshtml` but modify)
6. Create new folder `Controllers/api` and new controller `CategoryController`
   - Create constructor with input `ApplicationDbContext`
   - Create `HttpPost` action `Add(Category dto)` with `Route("add")`
   - Inside action, instantiate new `Category` object using dto fields, add to database and save
   ```cs
   public IActionResult Add(Category dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            DisplayOrder = dto.DisplayOrder
        };
        _db.Categories.Add(category);
        _db.SaveChanges();
        return Ok();
    }
   ```
   - Return `Ok`
7. Add JS section to `ListAndCreate.cshtml`
   - Find create button and add `EventListener` for click event to trigger `createCategory`
   - Create async function `createCategory` using `fetch`

```js
@section Scripts {
    <script>
        var createBtn = document.getElementById("createBtn");
        createBtn.addEventListener("click", createCategory);

        async function createCategory() {
          await fetch('/api/Category/add', {
                method: 'POST',
                headers: {
                    'Content-type': 'application/json',
                    'Accept': '*/*'
                },
                body: JSON.stringify({
                    Name: document.getElementById("nameInput").value,
                    DisplayOrder: document.getElementById("orderInput").value,
                })
            });
        }
    </script>
}
```

8. In `Controllers/api/CategoryController`, add new `HttpGet` action `Get()`
   - Find all `Category`
   - Return `Ok`
9. In `createCategory()` in `ListAndCreate.cshtml`, add another GET fetch to get all category and update the table

```js
await fetch("/api/Category/get", {
  method: "GET",
  headers: {
    "Content-type": "application/json",
    Accept: "*/*",
  },
})
  .then((data) => data.json())
  .then((result) => {
    tableElement.querySelector("tbody").innerHTML = ``;
    result.forEach((category) => {
      const row = document.createElement("tr");
      row.innerHTML = `
                        <td>${category.name}</td>
                        <td>${category.displayOrder}</td>
                        <td>
                            <div class="w-75 btn-group" role="group">
                                <a asp-action="Edit" asp-controller="Category" asp-route-id="${category.id}" class="btn btn-primary">Edit</a>
                                <a asp-action="Delete" asp-controller="Category" asp-route-id="${category.id}" class="btn btn-danger">Delete</a>
                            </div>
                        </td>
                    `;
      tableElement.querySelector("tbody").appendChild(row);
    });
  });
```
