using CloudCustomers.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace CloudCustomers.Services
{
    public interface IUsersService
    {
        public Task<List<User>> GetAllUsers();
    }

    public class UsersService : IUsersService
    {
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _distributedcache;

        public UsersService(IDistributedCache distributedcache, HttpClient httpClient)
        {
            _distributedcache = distributedcache;
            _httpClient = httpClient;
        }
        public async Task<List<User>> GetAllUsers() 
        {
            string cacheKey = "users";
            List<User> users = new List<User>();

            //Try to get the users from cache
             var cachedUsers = await _distributedcache.GetStringAsync(cacheKey);

            if (cachedUsers != null)
            {
                // if found in cache
                users = JsonSerializer.Deserialize<List<User>>(cachedUsers);
                return users;
            }

            var userResponse = await _httpClient.GetAsync("https://dummyjson.com/users");
            if (userResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return users;
            }

            var responseContent = userResponse.Content;
            var result = await responseContent.ReadFromJsonAsync<UsersCollection>();

            if (result != null && result.Users.Any())
            {
                // Set the users in the cache
                var jsonData = JsonSerializer.Serialize(result.Users.ToList());
                await _distributedcache.SetStringAsync(cacheKey, jsonData);
                users = result.Users.ToList();
            }

                return users;
          
        }
    }

}
