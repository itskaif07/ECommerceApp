namespace ECommerceApp.Helpers
{
    public class NotificationHelper
    {

        public static (string message, string notificationType) GetNotificationMessage(string actionResult)
        {
            return actionResult.ToLower() switch
            {
                "add" => ("Item added to cart", "success"),
                "remove" => ("Item removed from cart", "error"),
                "insufficientstock" => ("Insufficient stock", "error"),
                "increase" => ("Quantity increased", "success"),
                "decrease" => ("Quantity decreased", "error"),
                "ordercomplete" => ("Order Completed successfully", "success"),
                "orderdelete" => ("Order Deleted", "error"),
                "minimumprice" => ("Order must be of at least ₹100.", "error"),
                "addreview" => ("Review added", "success"),
                _ => ("Unknown action", "info")
            };
        }

        public static string FormatNotification(string message, string notificationType)
        {
            string cssClass = GetNotificationCss(notificationType);
            return $"<div class='{cssClass} px-10 py-2 text-center'>{message}</div>";
        }

        public static string GetNotificationCss(string notificationType) =>
            notificationType.ToLower() switch
            {
                "success" => "bg-green-600 text-white",
                "error" => "bg-red-500 text-white",
                "info" => "bg-blue-500 text-white",
                "warning" => "bg-yellow-500 text-black",
                _ => "bg-white text-black"
            };
    }
}
