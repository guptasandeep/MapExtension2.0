XA.component.customLocationRatingAndReview = (function ($, document) {
/* eslint-disable */
    "use strict";
    var api = {},
        scriptsLoaded = false;
			
	api.init = function() {
		if ($("body").hasClass("on-page-editor")) {
		return;
		}
		if(!scriptsLoaded && document.querySelector('.component.map'))
		{
			scriptsLoaded = true;
			api.listenInfowindowOpenEvent();
		}	
    };	
	
	api.setRatingAndReviewBySearchText = function(ratingAndReviewDivId, noRatingAndReviewDivId, searchText) {
		var ratingAndReviewDiv = $(`#${ratingAndReviewDivId}`);
		var noRatingAndReviewDiv = $(`#${noRatingAndReviewDivId}`);
		if(!(ratingAndReviewDiv.html().indexOf('#average_rating#') > -1) && !(ratingAndReviewDiv.html().indexOf('#review_count#') > -1))
			return;
		
		var request = {query: searchText, fields: ['place_id']};
		var sxamap = XA.connector.mapsConnector.getMaps()[$('.map-canvas').attr('id')];
		var service = new google.maps.places.PlacesService(sxamap);
		service.findPlaceFromQuery(request, requestPlaceDetails(ratingAndReviewDiv, noRatingAndReviewDiv));
	}

	function requestPlaceDetails(ratingAndReviewDiv, noRatingAndReviewDiv) {
		return function (results, status) {
			if (status === google.maps.places.PlacesServiceStatus.OK) {
				var request = {
					placeId: results[0].place_id,
					fields: ['rating', 'user_ratings_total','reviews']
				};
				var sxamap = XA.connector.mapsConnector.getMaps()[$('.map-canvas').attr('id')];
				var service = new google.maps.places.PlacesService(sxamap);
				service.getDetails(request, getRatingAndReviewByPlaceID(ratingAndReviewDiv, noRatingAndReviewDiv));
			}
		};
	}

	function getRatingAndReviewByPlaceID(ratingAndReviewDiv, noRatingAndReviewDiv) {
		return function (place, status) {
			if (status == google.maps.places.PlacesServiceStatus.OK) {
				if(noRatingAndReviewDiv.length > 0 && ratingAndReviewDiv.length > 0)
				{
					if (place.rating != undefined && place.user_ratings_total != undefined) {
						ratingAndReviewDiv.toggleClass('ratings-and-reviews');
						ratingAndReviewDiv.removeClass('d-none');
						ratingAndReviewDiv.html(ratingAndReviewDiv.html().replace('#average_rating#', `${place.rating.toFixed(1)} <i data-star="${place.rating.toFixed(1)}"></i>`).replace('#review_count#', place.user_ratings_total));
						noRatingAndReviewDiv.remove();
					}
				}
			}
		};
	}

	api.listenInfowindowOpenEvent = function() {
		$(document).on('infowindowOpen', function(event, infoWindow) {
			var openedInfoWindowsRatingReviewMainDiv = $('.rating-and-review-main');
			if(openedInfoWindowsRatingReviewMainDiv.length > 0) {
				openedInfoWindowsRatingReviewMainDiv.each(function() {
				var ratingReviewMainDivId = $(this).attr("id");
				var poiId = ratingReviewMainDivId.replace('-rating-and-review-main','');
				var ratingReviewDivId = `${poiId}-rating-and-review`;
				var noRatingReviewDivId = `${poiId}-no-rating-and-review`;
				var locationTitleText = $(`#${ratingReviewMainDivId} .title`).text();
					api.setRatingAndReviewBySearchText(ratingReviewDivId,noRatingReviewDivId,locationTitleText);
				});				
			}
		});
	}
		
    return api;

}(jQuery, document));

XA.register("CustomLocationRatingAndReview", XA.component.customLocationRatingAndReview);