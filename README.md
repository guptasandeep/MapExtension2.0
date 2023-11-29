# Sandeepkumar Gupta's blog post contribution

Following are the instructions to test the source code shown in the blog posts.
- https://blogs.perficient.com/author/gsandeepkumar/ 

## PreRequisites

- Sitecore 10.3 Update 1 with SXA installed in local
- Developer workstation to validate the solution
- Google Map provider key. Make sure the Places API and Maps Javascript API are enabled on the key.

## Setup

1. Clone the below repositories 

https://github.com/guptasandeep/MapExtension2.0.git

2. Take backup of webroot\App_Config and webroot\bin folders. 

3. Deploy the files.
   In cloned repositories,
- Copy all the binaries from https://github.com/guptasandeep/MapExtension2.0/tree/master/setup/bin_binaries to the Sitecore instance bin folder.
- Copy Include folder from https://github.com/guptasandeep/MapExtension2.0/tree/master/setup/App_Config to the Sitecore instance App_Config folder.
- Restore the Sitecore Master database .bacpac file from https://github.com/guptasandeep/MapExtension2.0/blob/master/Database/sitecorethinker1031_Master_29_Nov_2023.bacpac
- Update the connection strings accordingly.
- Update the IIS Site bindings and host file for the local domain - sitecorethinker1031sc.dev.local.

127.0.0.1 sitecorethinker1031sc.dev.local
4. Publish all the items.

3. From the Sitecore control panel > please do the Populate Solr Managed Schema and Rebuild all the indexes.

## Contributions on the Perficient Blog https://blogs.perficient.com/author/gsandeepkumar/

### Test pages 

Following are the blog post URLs and local test URLs which one can use to test it.

1. SXA Map component Part 6 Google average rating and total reviews
- Blog URL	: https://blogs.perficient.com/2023/10/25/sxa-map-google-average-rating-and-total-reviews/
- Test URL	: https://sitecorethinker1031sc.dev.local/Location%20Demo/Google%20review%20and%20rating%20demo/Map

2. SXA Map component Part 7 Most relevant Google reviews
- Blog URL	: https://blogs.perficient.com/2023/10/31/sxa-map-component-part-7-most-relevant-google-reviews/
- Test URL	: https://sitecorethinker1031sc.dev.local/Location%20Demo/Google%20review%20and%20rating%20demo/Map%20With%20Reviews

3. SXA Map component Part 8 Google rating and reviews in Search Results
- Blog URL	: https://blogs.perficient.com/2023/10/25/sxa-map-google-average-rating-and-total-reviews/
- Test URL	: https://sitecorethinker1031sc.dev.local/Location%20Demo/Google%20review%20and%20rating%20demo/Location%20Search%20Result#g=42.3600825|-71.0588801&o=Distance%2CAscending&a=Boston%2C%20MA%2C%20USA
