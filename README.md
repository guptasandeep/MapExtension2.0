# Sandeepkumar Gupta blog posts contribution

Following are the instructions to test the source code shown in the blog posts.
- https://blogs.perficient.com/author/gsandeepkumar/ 

## PreRequisites

- Sitecore 10.3 with SXA installed in local
- Developer workstation to validate the solution
- Google Map provider key. Make sure the Places API and Maps Javascript API are enabled on the key.

## Setup

1. Clone the below repositories 

https://github.com/guptasandeep/MapExtension2.0.git

https://github.com/guptasandeep/SitecoreThinker.git

2. Take backup of webroot\App_Config and webroot\bin folders. 

3. In cloned repositories, 
- Build both the solutions
- Copy the dlls - CustomSXA.Foundation.Search.dll and CustomSXA.Foundation.MapExtension.dll to your webroot\bin
- Copy the App_Config files from both the solutions to your webroot\App_Config

4. Restore the .\Database\sitecore1031_master.bacpac or install the tenant package .\Database\20231102.2307.demotenant-1.zip

5. Make local host entry of sitecorethinker1031sc.dev.local

127.0.0.1 sitecorethinker1031sc.dev.local

6. In the IIS site node, add the binding of sitecorethinker1031sc.dev.local

7. Log in to Sitecore, traverse to the below item, and provide the valid Google Map provider key in the field Key. Save and do the full site publish.
- /sitecore/content/demotenant/sitecorethinker/Settings/Maps Provider

8. From the Sitecore control panel > please do the Populate Solr Managed Schema and Rebuild all the indexes.

## Contributions on the Perficient Blog https://blogs.perficient.com/author/gsandeepkumar/

### Test pages 

Following are the blog post URLs and local test URLs which one can use to test it.

1. SXA Map component Part 6 Google average rating and total reviews
- Blog URL	: https://blogs.perficient.com/2023/10/25/sxa-map-google-average-rating-and-total-reviews/
- Test URL	: https://sitecorethinker1031sc.dev.local/Location%20Demo/Google%20review%20and%20rating%20demo/Map

2. SXA Map component Part 7 Most relevant Google reviews
- Blog URL	: https://blogs.perficient.com/2023/10/25/sxa-map-google-average-rating-and-total-reviews/
- Test URL	: https://sitecorethinker1031sc.dev.local/Location%20Demo/Google%20review%20and%20rating%20demo/Map%20With%20Reviews
