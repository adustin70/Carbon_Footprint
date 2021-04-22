const API_URL = 'https://api.v2.emissions-api.org/api/v2/carbonmonoxide/geo.json' +
    '?country=US&begin=2021-03-19&end=2021-03-21';

new deck.DeckGL({
    container: 'container',
    mapboxApiAccessToken: "pk.eyJ1IjoiYWR1c3RpbiIsImEiOiJja21tZTdyczEwZW1sMm9ueGVuN2oxeTlqIn0.O7RVdSgwx_89pIhFA0nWuA",
    mapStyle: "mapbox://styles/mapbox/dark-v10",
    longitude: -97,
    latitude: 40,
    zoom: 3,
    pitch: 0,
    layers: [
        new deck.HexagonLayer({
            extruded: true,
            radius: 30000,
            data: API_URL,
            dataTransform: d => d.features,
            elevationScale: 300,
            getColorValue: points => points.reduce((sum, point) => sum + point.properties.value, 0) / points.length,
            getElevationValue: points => points.reduce((sum, point) => sum + point.properties.value, 0) / points.length,
            getPosition: d => d.geometry.coordinates,
        }),
    ]
});