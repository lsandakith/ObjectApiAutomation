using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Objects
{
    public class Object
    {
        static async Task Main(string[] args)
        {
            await TestGetAllObjects();
            string objectId = await TestAddObject();
            await TestGetObjectById(objectId);
            await TestPatchObject(objectId);
            await TestDeleteObject(objectId);
        }
        // Get List of all Objects
        static async Task TestGetAllObjects()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://api.restful-api.dev/objects";
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("GET request successful. Response:");
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseBody);
                    List<object> objects = JsonConvert.DeserializeObject<List<object>>(responseBody);


                    foreach (var obj in objects)
                    {

                        dynamic dynamicObject = obj;


                        Assert.IsNotNull(dynamicObject.id, "ID should not be null");


                        Assert.IsNotNull(dynamicObject.name, "Name should not be null");


                        Assert.IsNotNull(dynamicObject.data, "Data should not be null");


                        if (dynamicObject.data != null)
                        {
                            dynamic data = dynamicObject.data;


                            if (data.color != null)
                            {
                                Assert.IsNotNull(data.color, "Color should not be null");
                            }


                            if (data.capacity != null)
                            {
                                Assert.IsNotNull(data.capacity, "Capacity should not be null");
                            }


                            if (data.price != null)
                            {
                                Assert.IsNotNull(data.price, "Price should not be null");
                            }
                        }
                    }

                    Console.WriteLine("All assertions passed.");
                    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                }
                else
                {
                    Console.WriteLine($"GET request failed with status code {response.StatusCode}");
                }
            }
        }

        // ADD Object (Post Request)
        static async Task<string> TestAddObject()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://api.restful-api.dev/objects";
                string requestBody = @"{
               ""name"": ""Apple MacBook Pro 16"",
               ""data"": {
                  ""year"": 2019,
                  ""price"": 1849.99,
                  ""CPU model"": ""Intel Core i9"",
                  ""Hard disk size"": ""1 TB""
               }
            }";

                HttpContent content = new StringContent(requestBody);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("POST request successful. Response:");
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseBody);

              
                    dynamic responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);
                    
                    Assert.IsNotNull(responseObject.id, "ID should not be null");
                    Assert.IsNotNull(responseObject.name, "Apple MacBook Pro 16");
                    Assert.IsNotNull(responseObject.data, "Data should not be null");


                    if (responseObject.data != null)
                    {
                        dynamic data = responseObject.data;
                        Assert.IsNotNull(data.year);
                        Assert.IsNotNull(data.price);
                        Assert.IsNotNull(data["CPU model"]);
                        Assert.IsNotNull(data["Hard disk size"]);
                    }

                    Console.WriteLine("All assertions passed.");

                    string objectId = responseObject.id;
                    return objectId;
                }
                else
                {
                    Console.WriteLine($"POST request failed with status code {response.StatusCode}");
                    return null;
                }
            }
        }

        // Get a single object using the above added ID
        static async Task TestGetObjectById(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"https://api.restful-api.dev/objects/{id}";
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"GET request successful for object with ID {id}. Response:");
                    string responseBody = await response.Content.ReadAsStringAsync();

                    dynamic responseObject = JsonConvert.DeserializeObject(responseBody);


                    Assert.IsNotNull(responseObject.id, "ID should not be null");
                    Assert.IsNotNull(responseObject.name, "Name should not be null");
                    Assert.IsNotNull(responseObject.data, "Data should not be null");


                    dynamic data = responseObject.data;
                    Assert.IsNotNull(data.year);
                    Assert.IsNotNull(data.price);
                    Assert.IsNotNull(data["CPU model"]);
                    Assert.IsNotNull(data["Hard disk size"], "Hard disk size should not be null");

                    Console.WriteLine("All assertions passed For Get Single Object");
                    Console.WriteLine(responseBody);
                }
                else
                {
                    Console.WriteLine($"GET request failed for object with ID {id} with status code {response.StatusCode}");
                }
            }
        }

        //  Update the object added in Step 2 using PUT
        static async Task TestPatchObject(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"https://api.restful-api.dev/objects/{id}";
                string requestBody = @"{
               ""name"": ""Apple MacBook Pro 16 (Updated Name)""
            }";

                HttpContent content = new StringContent(requestBody);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = await client.PatchAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"PATCH request successful for object with ID {id}. Response:");

                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic responseObject = JsonConvert.DeserializeObject(responseBody);


                    Assert.IsNotNull(responseObject.id, "ID should not be null");
                    Console.WriteLine(responseObject);
                    Console.WriteLine("Object updated successfully.");
                 
                }
                else
                {
                    Console.WriteLine($"PATCH request failed for object with ID {id} with status code {response.StatusCode}");
                }
            }
        }

        // Delete the object using ID
        static async Task TestDeleteObject(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"https://api.restful-api.dev/objects/{id}";

                HttpResponseMessage response = await client.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"DELETE request successful for object with ID {id}");
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic responseObject = JsonConvert.DeserializeObject(responseBody);

                    Assert.IsNotNull(responseObject.message);
                    Console.WriteLine("All assertions passed.");
                    Console.WriteLine($"Object with id ={id} has been deleted.", responseObject.message);
                    Console.WriteLine(responseObject);
                }
                else
                {
                    Console.WriteLine($"DELETE request failed for object with ID {id} with status code {response.StatusCode}");
                }
            }
        }
    }
}
