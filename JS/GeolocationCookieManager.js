XA.component.geolocationcookiemanger = (function ($, document) {

    "use strict";
    var api = {},
        scriptsLoaded = false;
		
	api.init = function() {
		if ($("body").hasClass("on-page-editor")) {
		return;
		}

		if(!scriptsLoaded)
		{
			scriptsLoaded = true;
			XA.component.search.vent.on("hashChanged", function(hash) {
				var reloadPage = false;
				
				var components = $('*[data-properties]');
				_.each(components, function(elem) {
				var $el = $(elem),
                properties = $el.data("properties");
					var signature = properties.searchResultsSignature;
					if (typeof signature !== "undefined")
					{
						var gsign = signature.length > 0 && signature !== "" ? signature + "_g" : "g";
						var gsignVal = hash[gsign];
						if (typeof gsignVal !== "undefined" && gsignVal !== null && gsignVal !== "") {
							if(XA.cookies.readCookie(gsign) !== gsignVal)
							{
								XA.cookies.createCookie(gsign, gsignVal);
								reloadPage = true;
							}
						}		
					}
				});
				if(reloadPage)
				{					
					window.location.reload(); //needed to send the newly updated cookie to server so the map loads with the distance value
				}
            });		
		}	
    };	
  
    return api;

}(jQuery, document));

XA.register("geolocationcookiemanger", XA.component.geolocationcookiemanger);