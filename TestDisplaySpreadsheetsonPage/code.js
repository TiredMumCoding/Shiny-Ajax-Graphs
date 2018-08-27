var json
var stringFromJSON


function init() {


    json = document.getElementById("jsonLabel").innerHTML
    stringFromJson = JSON.parse(json)
 

    var margin = { top: 30, right: 20, bottom: 30, left: 50 },
        width = 600 - margin.left - margin.right,
        height = 270 - margin.top - margin.bottom;

    var parseDate = d3.timeParse("%d/%m/%Y");

    for (i = 0; i < stringFromJson.length; i++) {
        stringFromJson[i].theDate = parseDate(stringFromJson[i].theDate)
    }

    ///aggregating and summing the data
    var theData = d3.nest()
        // set the key.  In D3 terms, this is the thing we'll be grouping on. rollup is the thing being summed
        .key(function (d) { return d.theDate; })
        .rollup(function (d) {
            return d3.sum(d, function (g) { return g.theCount; });
        }).entries(stringFromJson);


    //set the scales for the graph
    var x = d3.scaleTime().range([0, width])
    var y = d3.scaleLinear().range([height, 0])

    //scale the range of the data
    x.domain(d3.extent(stringFromJson, function (stringFromJson) { return stringFromJson.theDate }));
    y.domain([0, d3.max(theData, function (theData) { return theData.value})]);

    // define the line
    var valueLine = d3.line()
        .x(function (theData) { return x(new Date(theData.key)); })
        .y(function (theData) { return y(theData.value); })


    // add svg element and position on the page
    var svg = d3.select("#graph").append("svg")
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
        // g is a group element that's going to be the reference for everything else.
        .append("g")
        .attr("transform",
            "translate(" + margin.left + "," + margin.top + ")")

    //draw something
    // Add the valueline path.
    svg.append("path")
        .data([theData])
        .attr("class", "line")
        .attr("d", valueLine);


    // Add the X Axis
    svg.append("g")
        .attr("class", "axis")
        .attr("transform", "translate(0," + height + ")")
        .call(d3.axisBottom(x));

    // Add the Y Axis
    svg.append("g")
        .attr("class", "axis")
        .call(d3.axisLeft(y));

    // Add the scatterplot
   svg.selectAll("dot")
       .data(theData)
       .enter().append("circle")
      .attr("r", 5)
       .attr("cx", function (theData) { return x(new Date(theData.key)); })
       .attr("cy", function (theData) { return y(theData.value); });

}

function Refresh() {
    $.ajax({
        type: 'POST',
        url: 'http://localhost:59305/Web.asmx/getData',
        async: true,
        dataType: "text",
        success: function (result) {
            // gets rid of all html tags
            result = $(result).text();
            result = JSON.parse(result)

            stringFromJson = JSON.parse(json)

            // for every item in results array
            for (i = 0; i < result.length; i++) {
                var match = false
                //compare with every item in the array that was used to build the last graph
                for (p = 0; p < stringFromJson.length; p++) {
                    if (result[i].ID === stringFromJson[p].ID) {match = true
                    break }
                    
                }
                if (!match) {
                    stringFromJson.push(result[i])
                    document.getElementById("jsonLabel").innerHTML = JSON.stringify(stringFromJson)
                    document.getElementById("graph").innerHTML = ""
                    init()
                }
            }



        },
        error: function () {
            alert("failed");
        }
    }); 

}

window.onload = init;
setInterval(Refresh, 5000)


