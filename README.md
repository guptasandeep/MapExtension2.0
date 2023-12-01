# Sandeepkumar Gupta's blog post contribution

Following are the instructions to test the source code shown in the blog posts.
- https://blogs.perficient.com/author/gsandeepkumar/
- https://sitecorethinker.wordpress.com/

## PreRequisites

- Sitecore 10.3 Update 1 with SXA installed in local
- Developer workstation to validate the solution
- Google Map provider key. Make sure the Places API and Maps Javascript API are enabled on the key.

## Setup

1. Clone the below repository 

https://github.com/guptasandeep/MapExtension2.0.git

2. Take a backup of webroot\App_Config and webroot\bin folders of your Sitecore Instance. 

3. Deploy the files.
   In cloned repositories,
- Copy all the binaries from https://github.com/guptasandeep/MapExtension2.0/tree/master/setup/bin_binaries to the Sitecore instance bin folder.
- Copy Include folder from https://github.com/guptasandeep/MapExtension2.0/tree/master/setup/App_Config to the Sitecore instance App_Config folder.
- Restore the Sitecore Master database .bacpac file from https://github.com/guptasandeep/MapExtension2.0/blob/master/Database/sitecorethinker1031_Master_29_Nov_2023.bacpac
- Update the connection strings accordingly.
- Update the IIS Site bindings and host file for the local domain - sitecorethinker1031sc.dev.local.
     - 127.0.0.1 sitecorethinker1031sc.dev.local
- Login to Sitecore
     - Provide a valid Google Map API key in the field Key of /sitecore/content/demotenant/sitecorethinker/Settings/Maps Provider. Key must enabled with Maps API, Places API, and Geocoding API services.
     - Ensure domain - sitecorethinker1031sc.dev.local is updated in Target Hostname and Hostname in /sitecore/content/demotenant/sitecorethinker/Settings/Site Grouping/sitecorethinker-cm.

4. Publish all the items.

3. From the Sitecore control panel > please do the Populate Solr Managed Schema and Rebuild all the indexes.

## Contributions on the Perficient Blog https://blogs.perficient.com/author/gsandeepkumar/

Following are the blog post URLs and local test URLs which one can use to test it.

1. Excluding and Including Sitecore Assemblies in Helix Publishing Pipeline Solution
- Blog URL	: https://blogs.perficient.com/2023/09/19/excluding-and-including-sitecore-assemblies-in-helix-publishing-pipeline-solution/

2. SXA Map component Part 6 Google average rating and total reviews
- Blog URL	: https://blogs.perficient.com/2023/10/25/sxa-map-google-average-rating-and-total-reviews/
- Test URL	: https://sitecorethinker1031sc.dev.local/Location%20Demo/Google%20review%20and%20rating%20demo/Map

3. SXA Map component Part 7 Most relevant Google reviews
- Blog URL	: https://blogs.perficient.com/2023/10/31/sxa-map-component-part-7-most-relevant-google-reviews/
- Test URL	: https://sitecorethinker1031sc.dev.local/Location%20Demo/Google%20review%20and%20rating%20demo/Map%20With%20Reviews

4. SXA Map component Part 8 Google rating and reviews in Search Results
- Blog URL	: https://blogs.perficient.com/2023/11/07/sxa-map-component-google-rating-and-reviews-in-search-results/
- Test URL	: https://sitecorethinker1031sc.dev.local/Location%20Demo/Google%20review%20and%20rating%20demo/Location%20Search%20Result#g=42.3600825|-71.0588801&o=Distance%2CAscending&a=Boston%2C%20MA%2C%20USA

5. Intermittent Issues with Sitecore Solr Search Results
- Blog URL	: https://blogs.perficient.com/2023/11/07/intermittent-issues-with-sitecore-solr-search-results/

6. Split SXA Sitemap into Multiple Sitemaps if the Size Limit is Exceeded
- Blog URL	: https://blogs.perficient.com/2023/11/14/split-sxa-sitemap-into-multiple-sitemaps-if-the-size-limit-is-exceeded/
- Please remove the domain sitecorethinker1031sc.dev.local from the Target Hostname and Hostname fields of /sitecore/content/demotenant/sitecorethinker/Settings/Site Grouping/sitecorethinker-cm. And update Target Hostname and Hostname with the domain sitecorethinker1031sc.dev.local in another Site group item - /sitecore/content/SitecoreThinkerTenant/sitecorethinker/Settings/Site Grouping/sitecorethinker.
- Publish all the items.
- Test URL	: https://sitecorethinker1031sc.dev.local/sitemap.xml

7. Enhance Sitecore 10.2 Sitemap
- Blog URL	: https://blogs.perficient.com/2023/11/23/enhance-sitecore-10-2-sitemap/
- This is for Sitecore 10.2. For setup, please refer https://github.com/guptasandeep/SitecoreThinker102
- Test URL	: https://sitecorethinker102sc.dev.local/sitemap.xml

## Contributions on my personal Sitecore Blog https://sitecorethinker.wordpress.com/2023/ 

8. SXA Search Components and Customization
- Blog URL	: https://sitecorethinker.wordpress.com/2023/11/29/sxa-search-components-and-customization/
- Ensure domain - sitecorethinker1031sc.dev.local is updated in Target Hostname and Hostname in /sitecore/content/demotenant/sitecorethinker/Settings/Site Grouping/sitecorethinker-cm and remove from /sitecore/content/SitecoreThinkerTenant/sitecorethinker/Settings/Site Grouping/sitecorethinker.
- Test URLs	:
   - https://sitecorethinker1031sc.dev.local/Search/Developers%20search%20demo
   - https://sitecorethinker1031sc.dev.local/Search/Developers%20Search%20Demo%20with%20Pagination

9. SXA Search component – Results Count
- Blog URL	: https://sitecorethinker.wordpress.com/2023/11/29/sxa-search-component-results-count/
- Test URL	: https://sitecorethinker1031sc.dev.local/Search/Developers%20search%20demo%20with%20custom%20result%20count

10. Deleting the Map Provider key from the database copy
- Blog URL	: https://sitecorethinker.wordpress.com/2023/11/29/deleting-the-map-provider-key-from-the-database-copy/

11. SXA Form submit breaks and multiple form submit calls issues
- Blog URL	: https://sitecorethinker.wordpress.com/2023/11/29/sxa-form-submit-breaks-and-multiple-form-submit-calls-issues/

12. Multilist field error with source query Sitecore 10.2
- Blog URL	: https://sitecorethinker.wordpress.com/2023/11/29/multilist-field-error-with-source-query-sitecore-10-2/

