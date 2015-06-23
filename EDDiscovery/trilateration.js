var Region = function(center) {
	var regionSize = 1;	// 1/32 Ly grid locations to search around candidate coordinate

	this.origins = [center];

	this.minx = Math.floor(center.x*32-regionSize)/32;
	this.maxx = Math.ceil(center.x*32+regionSize)/32;
	this.miny = Math.floor(center.y*32-regionSize)/32;
	this.maxy = Math.ceil(center.y*32+regionSize)/32;
	this.minz = Math.floor(center.z*32-regionSize)/32;
	this.maxz = Math.ceil(center.z*32+regionSize)/32;
	
	// number of grid coordinates in the region
	// for a region that has not been merged this is typically (2*regionSize)^3 (though if the center
	// was grid aligned it will be (2*regionsize-1)^3)
	this.volume = function() {
		return 32768*(this.maxx-this.minx+1/32)*(this.maxy-this.miny+1/32)*(this.maxz-this.minz+1/32);
	};

	// p has properties x, y, z. returns true if p is in this region, false otherwise
	this.contains = function(p) {
		return (p.x >= this.minx && p.x <= this.maxx
				&& p.y >= this.miny && p.y <= this.maxy
				&& p.z >= this.minz && p.z <= this.maxz);
	}
	
	// returns a new region that represents the union of this and r
	this.union = function(r) {
		if (!(r instanceof Region)) return null;

		var u = new Region({x: 0, y: 0, z: 0});
		u.origins = this.origins.concat(r.origins);
		u.minx = Math.min(this.minx, r.minx);
		u.miny = Math.min(this.miny, r.miny);
		u.minz = Math.min(this.minz, r.minz);
		u.maxx = Math.max(this.maxx, r.maxx);
		u.maxy = Math.max(this.maxy, r.maxy);
		u.maxz = Math.max(this.maxz, r.maxz);

		return u;
	};
	
	// returns the highest coordinate of the vector from p to the closest origin point. this is the
	// minimum value of region size (in Ly) that would include the specified point
	this.centrality = function(p) {
		var i, d, best = null;
		for (i = 0; i < this.origins.length; i++) {
			d = Math.max(
				Math.abs(this.origins[i].x - p.x),
				Math.abs(this.origins[i].y - p.y),
				Math.abs(this.origins[i].z - p.z)
			);
			if (d < best || best === null) best = d;
		}
		return best;
	}
	
	this.toString = function() {
		return "Region ["+this.minx+", "+this.miny+", "+this.minz
			+"] - ["+this.maxx+", "+this.maxy+", "+this.maxz+"] ("+this.volume()+" points)";
	};
};

var Trilateration = function () {
	this.distances = [];

	// dist is an object with properties x, y, z, distance
	this.addDistance = function(dist) {
		// check if dist has already been added - if so then don't add the distance
		for (var i=0; i < this.distances.length; i++) {
			var d = this.distances[i];
			if (d.x == dist.x && d.y == dist.y && d.z == dist.z && d.distance == dist.distance) {
				return;
			}
		}
		
		this.distances.push(dist);
		if (this.distances.length >= 3) run();
	};

	var self = this;

	function getRegions() {
		var regions = [];
		// find candidate locations by trilateration on all combinations of the input distances
		// expand candidate locations to regions, combining as necessary
		for (i1 = 0; i1 < self.distances.length; i1++) {
			for (i2 = i1+1; i2 < self.distances.length; i2++) {
				for (i3 = i2+1; i3 < self.distances.length; i3++) {
			 		var candidates = getCandidates(self.distances, i1, i2, i3);
			 		candidates.forEach(function(candidate) {
						var r = new Region(candidate);
						// see if there is existing region we can merge this new one into
						for (var j = 0; j < regions.length; j++) {
							var u = r.union(regions[j]);
							if (u.volume() < r.volume() + regions[j].volume()) {
								// volume of union is less than volume of individual regions so merge them
								regions[j] = u;
								// TODO should really rescan regions to see if there are other existing regions that can be merged into this
								r = null;	// clear r so we know not to add it
								break;
							}
						}
						if (r !== null) {
							regions.push(r);
						}
			 		});
				}
			}
		}

//		console.log("Candidate regions:");
//		$.each(regions, function() {
//			console.log(this.toString());
//		});
		return regions;
	}

	function checkDistances(p) {
		var count = 0;
		
		self.distances.forEach(function(dist) {
			var dp = 2;

			if (typeof dist.distance === 'string') {
				// if dist is a string then check if it has exactly 3 decimal places:
				if (dist.distance.indexOf('.') === dist.distance.length-4) dp = 3;
			} else if (dist.distance.toFixed(3) === dist.distance.toString()) {
				// assume it's 3 dp if its 3 dp rounded string matches the string version
				dp = 3;
			}

			if (dist.distance == eddist(p, dist, dp)) count++;
		});

		return count;
	}

	function run() {
		self.regions = getRegions();
		// check the number of matching distances for each grid location in each region
		// track the best number of matches (and the corresponding locations) and the next
		// best number
		self.bestCount = 0;
		self.best = [];
		self.nextBest = 0;
		self.next = [];

		self.regions.forEach(function(region) {
			region.bestCount = 0;
			region.best = [];
			region.nextBest = 0;
			region.next = [];
			for (var x = region.minx; x <= region.maxx; x+= 1/32) {
				for (var y = region.miny; y <= region.maxy; y+= 1/32) {
					for (var z = region.minz; z <= region.maxz; z+= 1/32) {
						var p = {x: x, y: y, z: z};
						var matches = checkDistances(p);
						if (matches > region.bestCount) {
							region.nextBest = region.bestCount;
							region.next = region.best;
							region.bestCount = matches;
							region.best = [p];
						} else if (matches === region.bestCount) {
							region.best.push(p);
						} else if (matches > region.nextBest) {
							region.nextBest = matches;
							region.next = [p];
						} else if (matches === region.nextBest) {
							region.next.push(p);
						}
						if (matches > self.bestCount) {
							self.nextBest = self.bestCount;
							self.next = self.best;
							self.bestCount = matches;
							self.best = [p];
						} else if (matches === self.bestCount) {
							var found = false;
							self.best.forEach(function(e) {
								if (e.x === p.x && e.y === p.y && e.z === p.z) {
									found = true;
									return false;
								}
							});
							if (!found) self.best.push(p);
						} else if (matches > self.nextBest) {
							self.nextBest = matches;
							self.next = [p];
						} else if (matches === self.nextBest) {
							var found = false;
							self.best.forEach(function(e) {
								if (e.x === p.x && e.y === p.y && e.z === p.z) {
									found = true;
									return false;
								}
							});
							if (!found) self.next.push(p);
						}
					}
				}
			}
		});
	}
};

// dists is an array of reference objects (properties x, y, z, distance)
// returns an object containing the best candidate found (properties x, y, z, totalSqErr, i1, i2, i3)
// i1, i2, i3 are the indexes into dists[] that were reference points for the candidate
// totalSqErr is the total of the squares of the difference between the supplied distance and the calculated distance to each system in dists[]
function getBestCandidate(dists) {
	var i1 = 0, i2 = 1, i3 = 2, i4;
	var bestCandidate = null;

	// run the trilateration for each combination of 3 reference systems in the set of systems we have distance data for
	// we look for the best candidate over all trilaterations based on the lowest total (squared) error in the calculated
	// distances to all the reference systems
	for (i1 = 0; i1 < dists.length; i1++) {
		for (i2 = i1+1; i2 < dists.length; i2++) {
			for (i3 = i2+1; i3 < dists.length; i3++) {
		 		var candidates = getCandidates(dists, i1, i2, i3);
		 		if (candidates.length == 2) {
					candidates[0].totalSqErr = 0;
					candidates[1].totalSqErr = 0;
					
		 			for (i4 = 0; i4 < dists.length; i4++) {
						var err = checkDist(candidates[0], dists[i4], dists[i4].distance);
		 				candidates[0].totalSqErr += err.error*err.error;
		 				err = checkDist(candidates[1], dists[i4], dists[i4].distance);
		 				candidates[1].totalSqErr += err.error*err.error;
		 			}
					if (bestCandidate === null || bestCandidate.totalSqErr > candidates[0].totalSqErr) {
						bestCandidate = candidates[0];
						bestCandidate.i1 = i1;
						bestCandidate.i2 = i2;
						bestCandidate.i3 = i3;
						//console.log("best candidate so far: (1st) "+JSON.stringify(bestCandidate,2));
					}
					if (bestCandidate.totalSqErr > candidates[1].totalSqErr) {
						bestCandidate = candidates[1];
						bestCandidate.i1 = i1;
						bestCandidate.i2 = i2;
						bestCandidate.i3 = i3;
						//console.log("best candidate so far: (2nd) "+JSON.stringify(bestCandidate,2));
					}
				}
			}
		}
	}
	return bestCandidate;
}

// dists is an array of reference objects (properties x, y, z, distance)
// i1, i2, i3 indexes of the references to use to calculate the candidates
// returns an array of two points (properties x, y, z). if the supplied reference points are disjoint then an empty array is returned
function getCandidates(dists, i1, i2, i3) {
	var p1 = dists[i1];
	var p2 = dists[i2];
	var p3 = dists[i3];
	
	var p1p2 = diff(p2, p1);
	var d = length(p1p2);
	var ex = scalarProd(1/d, p1p2);
	var p1p3 = diff(p3, p1);
	var i = dotProd(ex, p1p3);
	var ey = diff(p1p3, scalarProd(i, ex));
	ey = scalarProd( 1/length(ey), ey);
	var j = dotProd(ey, diff(p3, p1));

	var x = (p1.distance*p1.distance - p2.distance*p2.distance + d*d) / (2*d);
	var y = ((p1.distance*p1.distance - p3.distance*p3.distance + i*i + j*j) / (2*j)) - (i*x/j);
	var zsq = p1.distance*p1.distance - x*x - y*y;
	if (zsq < 0) {
		//console.log("inconsistent distances (z^2 = "+zsq+")");
		return [];
	} else {
		var z = Math.sqrt(zsq);
		var ez = crossProd(ex, ey);
		var coord1 = sum(sum(p1,scalarProd(x,ex)),scalarProd(y,ey));
		var coord2 = diff(coord1,scalarProd(z,ez));
		coord1 = sum(coord1,scalarProd(z,ez));
		return [coord1, coord2];
	}
}

// calculates the distance between p1 and p2 and then calculates the error between the calculated distance and the supplied distance.
// if dist has 3dp of precision then the calculated distance is also calculated with 3dp, otherwise 2dp are assumed
// returns and object with properties (distance, error, dp)
function checkDist(p1, p2, dist) {
	var ret = {dp: 2};

	if (typeof dist === 'string') {
		// if dist is a string then check if it has exactly 3 decimal places:
		if (dist.indexOf('.') === dist.length-4) ret.dp = 3;
	} else if (dist.toFixed(3) === dist.toString()) {
		// assume it's 3 dp if its 3 dp rounded string matches the string version
		ret.dp = 3;
	}

	ret.distance = eddist(p1, p2, ret.dp);
	ret.error = Math.abs(ret.distance - dist);
	return ret;
}

// dists is an array of reference objects (properties x, y, z, distance)
// p is a vector (properties x, y, z)
// returns the RMS error between the distances as calculated from the coordinates and the distances supplied
function getRMSError(p, dists) {
	var err = 0;
	$.each(dists, function() {
		var e = eddist(this, p) - this.distance;
		err += e*e;
	});
	return Math.sqrt(err/dists.length);
}

// returns a vector with the components of v rounded to 1/32
function gridLocation(v) {
	return {
		x: (Math.round(v.x*32)/32),
		y: (Math.round(v.y*32)/32),
		z: (Math.round(v.z*32)/32)
	};
}

function vectorToString(v) {
	if (v == null) return "(no coords)";
	return "("+v.x+", "+v.y+", "+v.z+")";
}

// copies the vector components (x, y, z properties) to object o
function setVector(o, v) {
	o.x = v.x;
	o.y = v.y;
	o.z = v.z;
}

function identicalVectors(v1, v2) {
	if (!('x' in v1) || !('x' in v2)) return false;
	return v1.x === v2.x && v1.y === v2.y && v1.z === v2.z;
}

// p1 and p2 are objects that have x, y, and z properties
// returns the scalar (dot) product p1 . p2
function dotProd(p1, p2) {
	return p1.x*p2.x + p1.y*p2.y + p1.z*p2.z;
}

// p1 and p2 are objects that have x, y, and z properties
// returns the vector (cross) product p1 x p2
function crossProd(p1, p2) {
	return {
		x: p1.y*p2.z - p1.z*p2.y,
		y: p1.z*p2.x - p1.x*p2.z,
		z: p1.x*p2.y - p1.y*p2.x
	};
}

// v is a vector obejct with x, y, and z properties
// s is a scalar value
// returns a new vector object containing the scalar product of s and v
function scalarProd(s, v) {
	return {
		x: s * v.x,
		y: s * v.y,
		z: s * v.z
	};
}

// p1 and p2 are objects that have x, y, and z properties
// returns the distance between p1 and p2
function dist(p1, p2) {
	return length(diff(p2,p1));
}

// v is a vector obejct with x, y, and z properties
// returns the length of v
function length(v) {
	return Math.sqrt(dotProd(v,v));
}

// p1 and p2 are objects that have x, y, and z properties
// dp is optional number of decimal places to round to (defaults to 2)
// returns the distance between p1 and p2, calculated as single precision (as ED does),
// rounded to the specified number of decimal places
function eddist(p1, p2, dp) {
	dp = (typeof dp === 'undefined') ? 2 : dp;
	var v = diff(p2,p1);
	var d = fround(Math.sqrt(fround(fround(fround(v.x*v.x) + fround(v.y*v.y)) + fround(v.z*v.z))));
	return round(d,dp);
}

// round to the specified number of decimal places
function round(v, dp) {
	return Math.round(v*Math.pow(10,dp))/Math.pow(10,dp);
}

// p1 and p2 are objects that have x, y, and z properties
// returns the difference p1 - p2 as a vector object (with x, y, z properties), calculated as single precision (as ED does)
function diff(p1, p2) {
	return {
		x: p1.x - p2.x,
		y: p1.y - p2.y,
		z: p1.z - p2.z
	};
}

// p1 and p2 are objects that have x, y, and z properties
// returns the sum p1 + p2 as a vector object (with x, y, z properties)
function sum(p1, p2) {
	return {
		x: p1.x + p2.x,
		y: p1.y + p2.y,
		z: p1.z + p2.z
	};
}

// dists is an array of four reference objects (properties x, y, z, distance)
// returns coordinate object (properties x, y, z)
function tunaMageCoords(dists) {
	var b = diff(dists[1], dists[0]);	// 2nd system relative to 1st system
	var c = diff(dists[2], dists[0]);	// 3rd system relative to 1st system
	var d = diff(dists[3], dists[0]);	// 4th system relative to 1st system

	var ea = dists[0].distance*dists[0].distance;
	var eb = dists[1].distance*dists[1].distance;
	var ec = dists[2].distance*dists[2].distance;
	var ed = dists[3].distance*dists[3].distance;

	var p = (ea-eb+b.x*b.x+b.y*b.y+b.z*b.z)/2;
	var q = (ea-ec+c.x*c.x+c.y*c.y+c.z*c.z)/2;
	var r = (ea-ed+d.x*d.x+d.y*d.y+d.z*d.z)/2;

	var ez =((p*d.x-r*b.x)*(b.y*c.x-c.y*b.x)/(b.y*d.x-d.y*b.x)-(p*c.x-q*b.x))/(((b.z*d.x-d.z*b.x)*(b.y*c.x-c.y*b.x)/(b.y*d.x-d.y*b.x))-(b.z*c.x-c.z*b.x));
	var ey =((p*c.x-q*b.x)-ez*(b.z*c.x-c.z*b.x))/(b.y*c.x-c.y*b.x);
	var ex =(p-ey*b.y-ez*b.z)/b.x;

	return sum(dists[0], {x: ex, y: ey, z: ez});
}

//-----------------------------------------------------------------------------------------
// Miscellaneous common functions
//-----------------------------------------------------------------------------------------

function updateSortArrow(event, data) {
// data.column - the index of the column sorted after a click
// data.direction - the sorting direction (either asc or desc)
	var th = $(this).find("th");
	th.find(".arrow").remove();
	var arrow = data.direction === "asc" ? "\u2191" : "\u2193";
	th.eq(data.column).append('<span class="arrow">' + arrow +'</span>');
}

// sort function that treats missing value (and values that can't be parsed as floats) as the largest values
function sortOptionalFloat(a,b) {
	if (isNaN(parseFloat(a))) {
		if (isNaN(parseFloat(b))) return 0;
		return 1;
	}
	if (isNaN(parseFloat(b))) return -1;
	return parseFloat(a)-parseFloat(b);
}

// sort function that treats missing value (and values that can't be parsed as integers) as the largest values
function sortOptionalInt(a,b) {
	if (isNaN(parseInt(a))) {
		if (isNaN(parseInt(b))) return 0;
		return 1;
	}
	if (isNaN(parseInt(b))) return -1;
	return parseInt(a)-parseInt(b);
}

// returns a string containing an sql insert statement for TradeDangerous
function getSQL(s) {
	var quotedName = s.name.replace(/'/g,"''");
	var d = (new Date()).toISOString().replace('T',' ').substr(0,19);
	return "INSERT INTO \"System\" VALUES(,'"+quotedName+"',"+s.x+","+s.y+","+s.z+",'"+d+"');\n";
}

// selects the contents of the current node (this)
// should be called in the context of the node to be selected (i.e. this === the node)
function selectAll() {
	if (window.getSelection) {
		var selection = window.getSelection();            
		var range = document.createRange();
		range.selectNodeContents(this);
		selection.removeAllRanges();
		selection.addRange(range);
	}
}

// returns a function that toggles the specified target element and changes the text of the
// this element based on the the current visibility of the target.
// the returned function can be set as a jQuery event handler
function getToggle(target, visibleText, hiddenText) {
	return function() {
		var $ctrl = $(this);
		$ctrl.text($(target).is(":visible") ? hiddenText : visibleText).attr("disabled", true);
		$(target).toggle("fast", function() {
			$ctrl.attr("disabled", false);
		});
	};
}
