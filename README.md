# CodeExercise

## **Requirements**
You are tasked with developing a system that allows us to query location. It should provide a function similar to this: SearchResult GetLocations(Location location, int maxDistance, int maxResults);

**Specifics**
- The system must be written in C#
- It must be possible to set a maximum distance
- It must be possible to set a maximum number of results
- Results should be ordered by distance
- The system should not slow down significantly if the number of locations increases


## **Architecture**
The project consists of an API, 'connecting' to a Service layer, which 'connects' to a 'DataLayer'. The API is configured to use OpenApi and as such can be used for testing the Location endpoint (the only endpoint). Additionally, I have included unit tests to test the basic service layer and the search logic in the repository layer along with some performance tests.

![gallery](https://raw.githubusercontent.com/Codendaal1120/CodeExercise/main/Documentation/overview.PNG)

#### **API**
The API receives the LocationService as an injected dependency, which in turns gets its dependencies injected (the LocationRepository). The API has very little responsibility and mainly is used to convert runtime interfaces to public models. In this case, the models are identical, but typically this is where I would add models with JSON attributes used to decorate the API payloads.

Since there is no authentication and only a single GET endpoint, the api can be accessed via the browser : 

![gallery](https://raw.githubusercontent.com/Codendaal1120/CodeExercise/main/Documentation/api.png)

#### **Service layer**
For the purposes of this exercise the service layer does not do much, since the querying (the main feature of the application) is done on the repository layer, but I thought it best to still included it. Typically, this is where I would add runtime logic.

#### **Repository layer**
This is where the magic happens and contains the search algorithms. In a real world scenario, I imagine that the locations would not be stored in memory, but rather on a DB, in which case the search logic will be encapsulated in the storage implementation. In our test case however, the storage is a CSV file, which gets loaded into memory.


## **Approach**
My approach to the searching was to implement a basic search first, and then evaluate the performance. I started by implementing a basic search function, using brute force, by looping each location, calculating the distance and then comparing the distance to the reference location. This approach does work, but as the size of the locations increases, so does the execution time. After some intense googling I found some articles which pointed me in the direction of geo hashing https://en.wikipedia.org/wiki/Geohash. This is a concept I am not familiar with, but I was keen to give it a go.
GeoHashing will allow me to create a hash string for a particular location, this can then be used to 'match' other locations containing the same hash, a.k.a they are close to each other. My plan was to build an index when the application starts, or loads the location data into memory, and then calculate a hash for each location. This would incur an initial cost, but subsequent queries should be faster. Initially my plan was to possibly sort the collection using the hash which would speed up my search query. The problem was that the location hash requires a precision, which is not directly translatable to distance. For me to honour the maxDistance in the search query, I would possibly need to calculate the hash on each query. Instead, what I ended up doing was to create a small precision (wider radius) and then group locations into regions, which I can use to reduce the initial search values. I would then rely on the brute force to filter within a region

### **Metrics**
![gallery](https://raw.githubusercontent.com/Codendaal1120/CodeExercise/main/Documentation/Perf.PNG)

## **Conclusion**
The performance increased significantly with the introduction of the hashing, but as I am relatively new to this topic I would warrant some further testing to confirm the accuracy.
