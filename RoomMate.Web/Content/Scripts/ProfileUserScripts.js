$(document).ready(function () {
    //open modal with edit user profile
    $("#editUser").click(function () {
        $("#displayUserInformation").children().hide();
        $("#editUserInformation").css('display', 'block');

        $("#cancelEditUser").css("display", "block");
        $("#editUser").css("display", "none");
    });
    //close modal with edit user profile
    $("#cancelEditUser").click(function () {
        $("#editUserInformation").css('display', 'none');
        $("#displayUserInformation").children().show();

        $("#cancelEditUser").css("display", "none");
        $("#editUser").css("display", "block");
    });
    //open delete modal
    $(".delete-modal").click(function () {
        $(".background-shadow").css('display', 'block')
        $(".center-modal").css('display', 'flex');
        //$(".delete-room-form").attr("href", $("delete-room-form").attr("href") + this.id)
        $(".modal-footer form").attr('action', $(".modal-footer form").attr("action") + "/" + this.id);
    });
    //close delete modal
    $(".close-delete-modal").click(function () {
        $(".background-shadow").css('display', 'none')
        $(".center-modal").css('display', 'none');
    });
    /*Edit room*/
    $("#updateRoom").click(function () {
        $(".editRoom").show();
        $(".displayRoom").hide();
    });
    /*Add room*/
    //add image in room
    
    $("#add-img-photo").click(function () {
        $("input[id='add-file']").click();
    });
    
    var imagesPreview = function (input, placeToInsertImagePreview) {

        if (input.files) {

            var filesAmount = input.files.length;

            for (i = 0; i < filesAmount; i++) {
                var reader = new FileReader();

                reader.onload = function (event) {
                    $($.parseHTML('<img>')).attr('src', event.target.result).appendTo(placeToInsertImagePreview);
                }

                reader.readAsDataURL(input.files[i]);
            }
        }

    };

    function validateImages(input) {
        const file = input.files[0];
        const fileType = file['type'];
        const validImageTypes = ['image/jpeg', 'image/png'];
        if (!validImageTypes.includes(fileType))
        {
            return false;
        }
        return true;
    }
    $('#add-file').on('change', function () {

        if (validateImages(this)) {
            var image = $(".add-room-img").children("img");
            image.remove();
            imagesPreview(this, 'div.add-room-img');
            $("#image-error").css('display', 'none');
        }
        else
        {
            $("#image-error").css('display', 'block');
        }

    });
    //disable button before searching room address
    $('#save-room').prop('disabled', true);
    //map
    var latlng;
    $("#searchCity").click(function () {
        latlng = null;
        var city = $("#city").val();
        var street = $("#street").val();
        var house = $("#house").val();
        var postCode = $("#postCode").val();
        var geocode = 'https://nominatim.openstreetmap.org/?addressdetails=1&street=' + street + '+' + house + '&city=' + city + '&postalcode=' + postCode + '&format=json&limit=1'
        //get json
        $.ajaxSetup({ async: false });
        $.getJSON(geocode, function (data) {
            latlng = [data[0].lon, data[0].lat]
        });
        //set new address
        if (latlng != null) {
            $('#save-room').prop('disabled', false);
            //set values to json
            var latlngJson = {
                lat: latlng[0],
                lng: latlng[1],
            }

            $.ajax({
                type: "POST",
                url: '/saveAddres',
                data: latlngJson,
                dataType: "json",
                success: function (data) {
                }
            });

            //
            $("#map").remove();
            $(".address-map").append('<div id="map" class="map"></div>');
            $("#popup").remove();
            $(".address-map").append('<div id="popup" class="ol-popup"></div>');
            $("#popup").append('<div id="popup-content"></div>');
            $("#popup").css("display", "block");

            var map = new ol.Map({
                target: 'map',
                layers: [
                    new ol.layer.Tile({
                        source: new ol.source.OSM()
                    })
                ],
                view: new ol.View({
                    center: ol.proj.fromLonLat([latlng[0], latlng[1]]),
                    zoom: 18
                })
            });

            var layer = new ol.layer.Vector({
                source: new ol.source.Vector({
                    features: [
                        new ol.Feature({
                            geometry: new ol.geom.Point(ol.proj.fromLonLat([latlng[0], latlng[1]]))
                        })
                    ]
                })
            });
            map.addLayer(layer);

            //set value to hiddenfor
            $("#SearchAddressLatitude").val(latlng[0]);
            $("#SearchAddressLongitude").val(latlng[1]);

            //set marker
            var container = document.getElementById('popup');
            var content = document.getElementById('popup-content');
            var overlay = new ol.Overlay({
                element: container,
                autoPan: true,
                autoPanAnimation: {
                    duration: 250
                }
            });
            map.addOverlay(overlay);
            content.innerHTML = '<p class="bold-txt">' + city + '</p><br />' + street + ' ' + house;
            overlay.setPosition(ol.proj.fromLonLat([latlng[0], latlng[1]]));

        }

    });
});