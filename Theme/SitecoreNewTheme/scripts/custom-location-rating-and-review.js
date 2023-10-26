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
						
						//On click of Rating and review text, open the Modal showing the recent reviews.  
						ratingAndReviewDiv.click(function(){
							var existingReviewModal = $(`${ratingAndReviewDiv.attr('id')}-modal`);
							var reviewModal;
							var exampleModel = $('#exampleModal');
							if(exampleModel.length == 0)
							{
								return;
							}
							
							if(existingReviewModal.length == 0)
							{
								var locationName = $(`#${ratingAndReviewDiv.attr('id')}-main .title`).text();
								var ratingAndReviewData = ratingAndReviewDiv.html();
								reviewModal = exampleModel.clone();
								reviewModal.attr('id',`${ratingAndReviewDiv.attr('id')}-modal`);
								reviewModal.removeClass('d-none');
								reviewModal.find('.modal-title').text(locationName);								
								reviewModal.find('.modal-rating-and-review').html(ratingAndReviewData);
								var commentTemplate = reviewModal.find('.google-review-comment');
								$.each(place.reviews, function(i, comment) {
									var reviewComment = commentTemplate.clone();
									reviewComment.find('.profile_photo_url').attr('src',comment.profile_photo_url);
									reviewComment.find('.author_name').text(comment.author_name);
									reviewComment.find('.author_name').attr('href',comment.author_url);
									reviewComment.find('.rating').html( `${comment.rating.toFixed(1)} <i data-star="${comment.rating.toFixed(1)}"></i>`);
									reviewComment.find('.relative_time_description').text(comment.relative_time_description);
									reviewComment.find('.time').text(getDateAndTimeFromTimestamp(comment.time));
									reviewComment.find('.text').text(comment.text);
									reviewComment.appendTo(reviewModal.find('.google-review-comment').parent());
									reviewComment.removeClass('d-none');
								});
								reviewModal.find('.more_reviews').attr("href",`https://www.google.com/maps?q=${encodeURIComponent(locationName)}`);
								reviewModal.find('.more_reviews').detach().appendTo(reviewModal.find('.google-review-comment').parent());
								reviewModal.appendTo($('exampleModal').parent());
							}
							else
							{
								reviewModal = existingReviewModal;
							}
							var myModal = new bootstrap.Modal(reviewModal, {
							  keyboard: false
							});
							myModal.toggle();
						});
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
	
	function getDateAndTimeFromTimestamp(timestamp)
	{
		var date = new Date(timestamp * 1000);
		var year = date.getFullYear();
		var month = date.getMonth() + 1; // Month is zero-based, so add 1
		var day = date.getDate();
		var hours = date.getHours();
		var minutes = date.getMinutes();
		var seconds = date.getSeconds();

		var formattedDate = year + '-' + month + '-' + day + ' ' + hours + ':' + minutes + ':' + seconds;
		return formattedDate;
	}
		
    return api;

}(jQuery, document));

XA.register("CustomLocationRatingAndReview", XA.component.customLocationRatingAndReview);