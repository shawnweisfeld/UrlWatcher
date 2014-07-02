UrlWatcher
==========
The goal of this application is to time how long it takes to download a file from a HTTP endpoint.

The TestEndpoint method downloads the file and then records the duration of the download to a csv file. It also captures the 
header information and places that into another text file. 

It will run "forever" in a loop till you kill the app.

Most changes you will need to make are in the Main method. 
Here you can list the endpoints you want to test along with the destination files names for the perf and header information. 
You can also control the sleep time between tests. 
