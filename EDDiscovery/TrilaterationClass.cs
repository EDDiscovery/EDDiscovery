using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2
{
    public class TrilaterationClass
    {
        class Coord
        {
            public double x,y,z;
        }


        double minx, maxx, miny, maxy, minz,maxz;
        Coord origins;

FIXME_VAR_TYPE Region(Coord center) 

{
	int regionSize= 1;	// 1/32 Ly grid locations to search around candidate coordinate

	this.origins = center;

	this.minx = Math.Floor(center.x*32-regionSize)/32;
	this.maxx = Math.Ceiling(center.x*32+regionSize)/32;
	this.miny = Math.Floor(center.y*32-regionSize)/32;
	this.maxy = Math.Ceiling(center.y*32+regionSize)/32;
	this.minz = Math.Floor(center.z*32-regionSize)/32;
	this.maxz = Math.Ceiling(center.z*32+regionSize)/32;
	
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

		FIXME_VAR_TYPE u= new Region({x: 0, y: 0, z: 0});
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

FIXME_VAR_TYPE Trilateration= function () {
	this.distances = [];

	// dist is an object with properties x, y, z, distance
	this.addDistance = function(dist) {
		// check if dist has already been added - if so then don't add the distance
		for (FIXME_VAR_TYPE i=0; i < this.distances.length; i++) {
			FIXME_VAR_TYPE d= this.distances[i];
			if (d.x == dist.x && d.y == dist.y && d.z == dist.z && d.distance == dist.distance) {
				return;
			}
		}
		
		this.distances.push(dist);
		if (this.distances.length >= 3) run();
	};

	FIXME_VAR_TYPE self= this;

	void  getRegions (){
		FIXME_VAR_TYPE regions= [];
		// find candidate locations by trilateration on all combinations of the input distances
		// expand candidate locations to regions, combining as necessary
		for (i1 = 0; i1 < self.distances.length; i1++) {
			for (i2 = i1+1; i2 < self.distances.length; i2++) {
				for (i3 = i2+1; i3 < self.distances.length; i3++) {
			 		FIXME_VAR_TYPE candidates= getCandidates(self.distances, i1, i2, i3);
			 		$.each(candidates, function() {
						FIXME_VAR_TYPE r= new Region(this);
						// see if there is existing region we can merge this new one into
						for (FIXME_VAR_TYPE j= 0; j < regions.length; j++) {
							FIXME_VAR_TYPE u= r.union(regions[j]);
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

	void  checkDistances (p){
		FIXME_VAR_TYPE count= 0;
		
		$.each(self.distances, function() {
			FIXME_VAR_TYPE dp= 2;

			if (typeof this.distance === 'string') {
				// if dist is a string then check if it has exactly 3 decimal places:
				if (this.distance.indexOf('.') === this.distance.length-4) dp = 3;
			} else if (this.distance.toFixed(3) === this.distance.toString()) {
				// assume it's 3 dp if its 3 dp rounded string matches the string version
				dp = 3;
			}

			if (this.distance == eddist(p, this, dp)) count++;
		});

		return count;
	}

	void  run (){
		self.regions = getRegions();
		// check the number of matching distances for each grid location in each region
		// track the best number of matches (and the corresponding locations) and the next
		// best number
		self.bestCount = 0;
		self.best = [];
		self.nextBest = 0;
		self.next = [];

		$.each(self.regions, function() {
			this.bestCount = 0;
			this.best = [];
			this.nextBest = 0;
			this.next = [];
			for (FIXME_VAR_TYPE x= this.minx; x <= this.maxx; x+= 1/32) {
				for (FIXME_VAR_TYPE y= this.miny; y <= this.maxy; y+= 1/32) {
					for (FIXME_VAR_TYPE z= this.minz; z <= this.maxz; z+= 1/32) {
						FIXME_VAR_TYPE p= {x: x, y: y, z: z};
						FIXME_VAR_TYPE matches= checkDistances(p);
						if (matches > this.bestCount) {
							this.nextBest = this.bestCount;
							this.next = this.best;
							this.bestCount = matches;
							this.best = [p];
						} else if (matches === this.bestCount) {
							this.best.push(p);
						} else if (matches > this.nextBest) {
							this.nextBest = matches;
							this.next = [p];
						} else if (matches === this.nextBest) {
							this.next.push(p);
						}
						if (matches > self.bestCount) {
							self.nextBest = self.bestCount;
							self.next = self.best;
							self.bestCount = matches;
							self.best = [p];
						} else if (matches === self.bestCount) {
							FIXME_VAR_TYPE found= false;
							$.each(self.best, function() {
								if (this.x === p.x && this.y === p.y && this.z === p.z) {
									found = true;
									return false;
								}
							});
							if (!found) self.best.push(p);
						} else if (matches > self.nextBest) {
							self.nextBest = matches;
							self.next = [p];
						} else if (matches === self.nextBest) {
							FIXME_VAR_TYPE found= false;
							$.each(self.best, function() {
								if (this.x === p.x && this.y === p.y && this.z === p.z) {
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
void  getBestCandidate (dists){
	FIXME_VAR_TYPE i1= 0, i2 = 1, i3 = 2, i4;
	FIXME_VAR_TYPE bestCandidate= null;

	// run the trilateration for each combination of 3 reference systems in the set of systems we have distance data for
	// we look for the best candidate over all trilaterations based on the lowest total (squared) error in the calculated
	// distances to all the reference systems
	for (i1 = 0; i1 < dists.length; i1++) {
		for (i2 = i1+1; i2 < dists.length; i2++) {
			for (i3 = i2+1; i3 < dists.length; i3++) {
		 		FIXME_VAR_TYPE candidates= getCandidates(dists, i1, i2, i3);
		 		if (candidates.length == 2) {
					candidates[0].totalSqErr = 0;
					candidates[1].totalSqErr = 0;
					
		 			for (i4 = 0; i4 < dists.length; i4++) {
						FIXME_VAR_TYPE err= checkDist(candidates[0], dists[i4], dists[i4].distance);
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
void  getCandidates (dists, i1, i2, i3){
	FIXME_VAR_TYPE p1= dists[i1];
	FIXME_VAR_TYPE p2= dists[i2];
	FIXME_VAR_TYPE p3= dists[i3];
	
	FIXME_VAR_TYPE p1p2= diff(p2, p1);
	FIXME_VAR_TYPE d= length(p1p2);
	FIXME_VAR_TYPE ex= scalarProd(1/d, p1p2);
	FIXME_VAR_TYPE p1p3= diff(p3, p1);
	FIXME_VAR_TYPE i= dotProd(ex, p1p3);
	FIXME_VAR_TYPE ey= diff(p1p3, scalarProd(i, ex));
	ey = scalarProd( 1/length(ey), ey);
	FIXME_VAR_TYPE j= dotProd(ey, diff(p3, p1));

	FIXME_VAR_TYPE x= (p1.distance*p1.distance - p2.distance*p2.distance + d*d) / (2*d);
	FIXME_VAR_TYPE y= ((p1.distance*p1.distance - p3.distance*p3.distance + i*i + j*j) / (2*j)) - (i*x/j);
	FIXME_VAR_TYPE zsq= p1.distance*p1.distance - x*x - y*y;
	if (zsq < 0) {
		//console.log("inconsistent distances (z^2 = "+zsq+")");
		return [];
	} else {
		FIXME_VAR_TYPE z= Math.sqrt(zsq);
		FIXME_VAR_TYPE ez= crossProd(ex, ey);
		FIXME_VAR_TYPE coord1= sum(sum(p1,scalarProd(x,ex)),scalarProd(y,ey));
		FIXME_VAR_TYPE coord2= diff(coord1,scalarProd(z,ez));
		coord1 = sum(coord1,scalarProd(z,ez));
		return [coord1, coord2];
	}
}

// calculates the distance between p1 and p2 and then calculates the error between the calculated distance and the supplied distance.
// if dist has 3dp of precision then the calculated distance is also calculated with 3dp, otherwise 2dp are assumed
// returns and object with properties (distance, error, dp)
void  checkDist (p1, p2, dist){
	FIXME_VAR_TYPE ret= {dp: 2};

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
void  getRMSError (p, dists){
	FIXME_VAR_TYPE err= 0;
	$.each(dists, function() {
		FIXME_VAR_TYPE e= eddist(this, p) - this.distance;
		err += e*e;
	});
	return Math.sqrt(err/dists.length);
}

// returns a vector with the components of v rounded to 1/32
void  gridLocation (v){
	return {
		x: (Math.round(v.x*32)/32),
		y: (Math.round(v.y*32)/32),
		z: (Math.round(v.z*32)/32)
	};
}

void  vectorToString (v){
	if (v == null) return "(no coords)";
	return "("+v.x+", "+v.y+", "+v.z+")";
}

// copies the vector components (x, y, z properties) to object o
void  setVector (o, v){
	o.x = v.x;
	o.y = v.y;
	o.z = v.z;
}

void  identicalVectors (v1, v2){
	if (!('x' in v1) || !('x' in v2)) return false;
	return v1.x === v2.x && v1.y === v2.y && v1.z === v2.z;
}

// p1 and p2 are objects that have x, y, and z properties
// returns the scalar (dot) product p1 . p2
void  dotProd (p1, p2){
	return p1.x*p2.x + p1.y*p2.y + p1.z*p2.z;
}

// p1 and p2 are objects that have x, y, and z properties
// returns the vector (cross) product p1 x p2
void  crossProd (p1, p2){
	return {
		x: p1.y*p2.z - p1.z*p2.y,
		y: p1.z*p2.x - p1.x*p2.z,
		z: p1.x*p2.y - p1.y*p2.x
	};
}

// v is a vector obejct with x, y, and z properties
// s is a scalar value
// returns a new vector object containing the scalar product of s and v
void  scalarProd (s, v){
	return {
		x: s * v.x,
		y: s * v.y,
		z: s * v.z
	};
}

// p1 and p2 are objects that have x, y, and z properties
// returns the distance between p1 and p2
void  dist (p1, p2){
	return length(diff(p2,p1));
}

// v is a vector obejct with x, y, and z properties
// returns the length of v
void  length (v){
	return Math.sqrt(dotProd(v,v));
}

// polyfill for fround (which is not implemented in node.js 0.10f.35)
FIXME_VAR_TYPE _f32= new Float32Array(1);
FIXME_VAR_TYPE fround= Math.fround || function(x) {
	return _f32[0] = x, _f32[0];
};

// p1 and p2 are objects that have x, y, and z properties
// dp is optional number of decimal places to round to (defaults to 2)
// returns the distance between p1 and p2, calculated as single precision (as ED does),
// rounded to the specified number of decimal places
void  eddist (p1, p2, dp){
	dp = (typeof dp === 'undefined') ? 2 : dp;
	FIXME_VAR_TYPE v= diff(p2,p1);
	FIXME_VAR_TYPE d= fround(Math.sqrt(fround(fround(fround(v.x*v.x) + fround(v.y*v.y)) + fround(v.z*v.z))));
	return round(d,dp);
}

// round to the specified number of decimal places
void  round (v, dp){
	return Math.round(v*Math.pow(10,dp))/Math.pow(10,dp);
}

// p1 and p2 are objects that have x, y, and z properties
// returns the difference p1 - p2 as a vector object (with x, y, z properties), calculated as single precision (as ED does)
void  diff (p1, p2){
	return {
		x: p1.x - p2.x,
		y: p1.y - p2.y,
		z: p1.z - p2.z
	};
}

// p1 and p2 are objects that have x, y, and z properties
// returns the sum p1 + p2 as a vector object (with x, y, z properties)
void  sum (p1, p2){
	return {
		x: p1.x + p2.x,
		y: p1.y + p2.y,
		z: p1.z + p2.z
	};
}

// dists is an array of four reference objects (properties x, y, z, distance)
// returns coordinate object (properties x, y, z)
void  tunaMageCoords (doube[] dists)
{
	var b= diff(dists[1], dists[0]);	// 2nd system relative to 1st system
	var c= diff(dists[2], dists[0]);	// 3rd system relative to 1st system
	var d= diff(dists[3], dists[0]);	// 4th system relative to 1st system

	var ea= dists[0].distance*dists[0].distance;
	var eb= dists[1].distance*dists[1].distance;
	var ec= dists[2].distance*dists[2].distance;
	var ed= dists[3].distance*dists[3].distance;

	var p= (ea-eb+b.x*b.x+b.y*b.y+b.z*b.z)/2;
	var q= (ea-ec+c.x*c.x+c.y*c.y+c.z*c.z)/2;
	var r= (ea-ed+d.x*d.x+d.y*d.y+d.z*d.z)/2;

	var ez=((p*d.x-r*b.x)*(b.y*c.x-c.y*b.x)/(b.y*d.x-d.y*b.x)-(p*c.x-q*b.x))/(((b.z*d.x-d.z*b.x)*(b.y*c.x-c.y*b.x)/(b.y*d.x-d.y*b.x))-(b.z*c.x-c.z*b.x));
	var ey=((p*c.x-q*b.x)-ez*(b.z*c.x-c.z*b.x))/(b.y*c.x-c.y*b.x);
	var ex=(p-ey*b.y-ez*b.z)/b.x;

	return sum(dists[0], {x: ex, y: ey, z: ez});
}

//-----------------------------------------------------------------------------------------
// Miscellaneous common functions
//-----------------------------------------------------------------------------------------

void  updateSortArrow (event, data){
// data.column - the index of the column sorted after a click
// data.direction - the sorting direction (either asc or desc)
	FIXME_VAR_TYPE th= $(this).find("th");
	th.find(".arrow").remove();
	FIXME_VAR_TYPE arrow= data.direction === "asc" ? "\u2191" : "\u2193";
	th.eq(data.column).append('<span class="arrow">' + arrow +'</span>');
}

// sort function that treats missing value (and values that can't be parsed as floats) as the largest values
void  sortOptionalFloat (a,b){
	if (isNaN(float.Parse(a))) {
		if (isNaN(float.Parse(b))) return 0;
		return 1;
	}
	if (isNaN(float.Parse(b))) return -1;
	return float.Parse(a)-float.Parse(b);
}

// sort function that treats missing value (and values that can't be parsed as integers) as the largest values
void  sortOptionalInt (a,b){
	if (isNaN(int.Parse(a))) {
		if (isNaN(int.Parse(b))) return 0;
		return 1;
	}
	if (isNaN(int.Parse(b))) return -1;
	return int.Parse(a)-int.Parse(b);
}

// returns a string containing an sql insert statement for TradeDangerous
void  getSQL (s){
	FIXME_VAR_TYPE quotedName= s.name.replace(/'/g,"''");
	FIXME_VAR_TYPE d= (new Date()).toISOString().replace('T',' ').substr(0,19);
	return "INSERT INTO \"System\" VALUES(,'"+quotedName+"',"+s.x+","+s.y+","+s.z+",'"+d+"');\n";
}

// selects the contents of the current node (this)
// should be called in the context of the node to be selected (i.e. this === the node)
void  selectAll (){
	if (window.getSelection) {
		FIXME_VAR_TYPE selection= window.getSelection();            
		FIXME_VAR_TYPE range= document.createRange();
		range.selectNodeContents(this);
		selection.removeAllRanges();
		selection.addRange(range);
	}
}

// returns a function that toggles the specified target element and changes the text of the
// this element based on the the current visibility of the target.
// the returned function can be set as a jQuery event handler
void  getToggle (target, visibleText, hiddenText){
	return function() {
		var $ctrl = $(this);
		$ctrl.text($(target).is(":visible") ? hiddenText : visibleText).attr("disabled", true);
		$(target).toggle("fast", function() {
			$ctrl.attr("disabled", false);
		});
	};
}
}


    }
}
