# Sandeepkumar Gupta blog posts contribution

Following are the instruction to test the source code shown in the blog posts.
- https://blogs.perficient.com/author/gsandeepkumar/ 

## PreRequisites

- Sitecore 10.2 with SXA installed in local
- Developer workstation to validate the solution
- Google Map provider key

## Setup

1. Clone below repositories 

https://github.com/guptasandeep/MapExtension.git

https://github.com/guptasandeep/SitecoreThinker.git

2. Take backup of webroot\App_Config and webroot\bin folders. 

3. In cloned repositories, 
- Build both the solutions
- Copy the dlls - CustomSXA.Foundation.Search.dll and CustomSXA.Foundation.MapExtension.dll to your webroot\bin
- Copy the App_Config files from both the solutions to your webroot\App_Config

4. Restore the .\Database\sitecorethinker_Master.bacpac or install the tenant package .\Database\20221126.1448.demotenant-1.zip

5. Make local host entry of sitecorethinkersc.dev.local

127.0.0.1 sitecorethinkersc.dev.local

6. In IIS site node, add the binding of sitecorethinkersc.dev.local

7. Login to Sitecore, traverse to below item and provide the valid Google Map provider key in the field Key. Save and do the full site publish.
- /sitecore/content/demotenant/sitecorethinker/Settings/Maps Provider

8. Rebuild all the indexes.

## Contributions on the Perficient Blog https://blogs.perficient.com/author/gsandeepkumar/

### Test pages 

Following has the blog post URLs and local test URLs which one can use to test it.

1. SXA Map component
- https://blogs.perficient.com/2022/06/29/sxa-map-component/
- Test URL: https://sitecorethinkersc.dev.local/Location%20Demo/Same%20Location%20Demo/Map

2. SXA Map component Part 2 With Search results and Location Finder
- https://blogs.perficient.com/2022/06/29/sxa-map-component-part-2-with-search-results-and-location-finder/
- Test URL: https://sitecorethinkersc.dev.local/Location%20Demo/Same%20Location%20Demo/Location%20Search%20Result#g=39.768403|-86.158068&o=null%2CAscending&a=Indianapolis%2C%20IN%2C%20USA

3. SXA Map component Part 3 Show distance in POI Marker
- https://blogs.perficient.com/2022/06/29/sxa-map-component-part-3-show-distance-in-poi-marker/
- Test URL: https://sitecorethinkersc.dev.local/Location%20Demo/Same%20Location%20Demo/Location%20Search%20Result#g=39.768403|-86.158068&o=null%2CAscending&a=Indianapolis%2C%20IN%2C%20USA

4. SXA Map component Part 4 Show POI markers for the same coordinates
- https://blogs.perficient.com/2022/07/31/sxa-map-component-part-4-show-poi-markers-for-the-same-coordinates/
- Test URL:
  - Scenario 1 https://sitecorethinkersc.dev.local/Location%20Demo/Same%20Location%20Demo/Location%20Search%20Result#g=39.768403|-86.158068&o=null%2CAscending&a=Indianapolis%2C%20IN%2C%20USA
  - Scenario 2 https://sitecorethinkersc.dev.local/Location%20Demo/Same%20Location%20Demo/Map

5. Custom SXA token for search scope query to support all the filter operations
- https://blogs.perficient.com/2022/08/28/custom-sxa-token-for-search-scope-query-to-support-all-the-filter-operations/
- Test URL: https://sitecorethinkersc.dev.local/Search%20Developers/Developers
  - ***Note: Please toggle the filter +, - and default i.e. no filter at Developer scope item and observe the different output on the actual Developer page.***

6. Query string-based custom SXA Search tokens for various search results scenarios
- https://blogs.perficient.com/2022/09/05/query-string-based-custom-sxa-search-tokens-for-various-search-results-scenarios/
- Test URL: https://sitecorethinkersc.dev.local/Search%20Developers/Developers#first%20name=Rakesh&last%20name=Gupta
  - ***Note: Please update the scope item for search result component and the query string as shown in the blog post for testing purpose.***

7. SXA Sort search results by distance for mapped multiple locations
- https://blogs.perficient.com/2022/09/19/sxa-sort-search-results-by-distance-for-mapped-multiple-locations/
- Test URL: https://sitecorethinkersc.dev.local/builders%20group%20projects%20listing#cg=25.6751129|94.10859980000001&o=Distance%2CAscending&a=Kohima%2C%20Nagaland%2C%20India

8. SXA Map component Part 5 Filter locations with Radius Filter or Custom Filter Slider components
- https://blogs.perficient.com/2022/11/19/sxa-map-component-part-5-filter-locations-with-radius-filter-or-custom-filter-slider-components/
- Test URL: https://sitecorethinkersc.dev.local/Location%20Demo/radius%20filter%20demo#g=39.768403|-86.158068&o=Distance%2CAscending&distance=1690&a=Indianapolis%2C%20IN%2C%20USA

## Contributions on my personal Sitecore Blog https://sitecorethinker.wordpress.com/2022/ 

9. Sitecore Forms Builder not working
- https://sitecorethinker.wordpress.com/2022/02/20/sitecore-forms-builder-not-working/

10. Indexes not appearing on the Index manager
- https://sitecorethinker.wordpress.com/2022/02/20/indexes-not-appearing-on-the-index-manager/

11. Search date filter Gotcha
- https://sitecorethinker.wordpress.com/2022/02/20/search-date-filter-gotcha/

12. Custom Sitecore Form element – Zip
- https://sitecorethinker.wordpress.com/2022/02/26/custom-sitecore-form-element-zip/

13. Custom Form element missing in Redirect to URL Parameters Form field dropdown
- https://sitecorethinker.wordpress.com/2022/02/27/custom-form-element-missing-in-redirect-to-url-parameters-form-field-dropdown/

14. Sitecore Form with Validation Summary
- https://sitecorethinker.wordpress.com/2022/02/27/sitecore-form-with-validation-summary/

15. Sitecore Scriban – Wrapping image with link using sc_beginfield and sc_endfield
- https://sitecorethinker.wordpress.com/2022/03/02/sitecore-scriban-wrapping-image-with-link-using-sc_beginfield-and-sc_endfield/

16. Sitecore 9.1 SIF errors
- https://sitecorethinker.wordpress.com/2022/06/12/sitecore-9-1-sif-errors/
