namespace RoyalVilla.DTO
{
    // we made this to have api response consistence 
    public class ApiResponse<TData> // when we create an obj of ApiResponce we have to define whether it's type is villa ,....
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public TData? Data { get; set; } // i bond the type of data's api to data prop (whatever it'll be the type of prop will be that)
        public object? Errors { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;


        // for avoiding to write a lot of codes for return types in action methods we made this method

        public static ApiResponse<TData> Create(bool success, int statusCode, string message, TData? data = default, object? errors = null)
        {
            return new ApiResponse<TData> 
            {
                Success = success, 
                StatusCode = statusCode, 
                Message = message, 
                Data = data, 
                Errors = errors 
            };

        }

        // instead of writing return for we used this structure to make short
        public static ApiResponse<TData> Ok(TData data, string message) =>
            Create(true, 200, message, data);

        public static ApiResponse<TData> CreatedAt(TData data, string message) =>
            Create(true, 201, message, data);

        public static ApiResponse<TData> NoContent(string message = "operation completed successfully") =>
            Create(true, 204, message);

        public static ApiResponse<TData> NotFound(string message = "resource not found") =>
            Create(false, 404, message);

        public static ApiResponse<TData> BadRequest(string message, object? errors = null) =>
            Create(false, 400, message, errors: errors);

        public static ApiResponse<TData> Conflict(string message) =>
            Create(false, 409, message);

        public static ApiResponse<TData> Error(int statusCode, string message, object? errors = null) =>
            Create(false, statusCode, message, errors: errors);

    }
}
