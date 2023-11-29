XA.component.customcomponentresultscount = (function($) {
    "use strict";
    const api = {
        initialized: false,
        init() {
            if (this.initialized) {
                return;
            }
            XA.component.search.vent.on("results-loaded", function (data) {
                let resultsDiv = $(".custom-results-count .results-count");
                if (resultsDiv) {
                    var signature = $(".search-results-count").data("properties");
                    if(signature){
                        signature = signature.targetSignature;
                    }
                    if (signature != "" && signature != data.searchResultsSignature) {
                        return;
                    }       
                    var resultsCountText = resultsDiv.html();
                    let rstart = 0;
                    let rend = 0;
                    let rcount= Number(data.dataCount);
                    let offset = Number(data.offset);
                    let pageSize = Number(data.pageSize);
                    if(rcount!=0 && rcount<=pageSize)
                    {
                        rstart = 1;
                        rend = rcount;
                    }
                    else if(rcount>pageSize)
                    {
                        rstart = offset + 1;
                        if(offset + pageSize >= rcount) //last page case and middle page case
                        {
                            rend = rcount;
                        }
                        else if(offset + pageSize < rcount) //middle page case
                        {
                            rend = offset + pageSize;
                        }
                    }
                    resultsCountText = resultsCountText.replace("{rstart}", rstart);
                    resultsCountText = resultsCountText.replace("{rend}", rend);
                    resultsCountText = resultsCountText.replace("{rcount}", rcount); 
                    resultsDiv.html(resultsCountText);                    
                    resultsDiv.show();
                }
            });
            this.initialized = true;  
        }    
    };
    return api;
}(jQuery));

XA.register("custom-component-results-count", XA.component.customcomponentresultscount); 