namespace Asset.Application.Common;

public static class ApiMessage
{
    // CURD Successful
    public const string SuccessfulCreate = "The item has been created successfully.";
    public const string SuccessfulUpdate = "The item has been updated successfully.";
    public const string SuccessfulDelete = "The item has been deleted successfully.";

    // CURD Failure
    public const string FailedCreate = "Failed to create the item. Please verify the details and try again later.";
    public const string FailedUpdate = "Failed to update the item. Please verify the details and try again later.";
    public const string FailedDelete = "Failed to delete the item. Please verify the details and try again later.";

    // Common
    public const string EntityIdMismatch = "The item IDs does not matched. Please check the details and try again.";

    // Not Fount
    public const string ItemNotFound = "The requested item could not be found. Please double-check the details and try again.";

    // No Items Found
    public const string NoItemsFound = "There are currently no items available.";
}
