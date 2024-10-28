define(["dojo/date/stamp"], function(stamp){

  console.log("loaded...");

	var seed = 9973;

	var randomNumber = function(range){
		var a = 8887;
		var c = 9643;
		var m = 8677;
		seed = (a * seed + c) % m;
		var res = Math.floor(seed / m * range);
		return res;
	};

	var chars = "0,1,2,3, ,4,5,6,7, ,8,9,a,b, ,c,d,e,f, ,g,h,i,j, ,k,l,m,n, ,k,o,p,q, ,r,s,t,u, ,v,w,x,y, ,z".split(',');
	var randomString = function(){
		var len = randomNumber(50), i, str = [];
		for(i = 0; i < len; ++i){
			str.push(chars[randomNumber(chars.length)]);
		}
		return str.join('');
	};

	var randomDate = function(){
		return new Date(randomNumber(10000000000000));
	};

	var generateItem = function(parentId, index, level){
		return {
			id: parentId + (index + 1),
			number: level <= 1 ? randomNumber(10000) : null,
			string: level <= 2 ? randomString() : null,
			date: level <= 3 ? stamp.toISOString(randomDate(), {selector: 'date'}) : null,
			time: stamp.toISOString(randomDate(), {selector: 'time'}),
			bool: randomNumber(10) < 5,
			hasPriorVersions: ((parentId + (index + 1)) === 'DocID_632294224800000000^usr2@brij.com^Table01^BrijCorp^3') ? true : false,
			isCateg: level <= 1 ? true : false,
		};
	};

	var generateLevel = function(parentId, level, maxLevel, maxChildrenCount, minChildrenCount){
		var i, item, res = [];
		var childrenCount = minChildrenCount + randomNumber(maxChildrenCount - minChildrenCount);
		for(i = 0; i < childrenCount; ++i){
			item = generateItem(parentId, i, level);
			res.push(item);
			if(level < maxLevel){
				item.children = generateLevel(item.id, level + 1, maxLevel, maxChildrenCount, minChildrenCount);
			}
		}
		return res;
	};

  const mGenPriors = (n) => {
    let idSlug = 'DocID_632294224800000000^usr2@brij.com^Table01^BrijCorp^';
    let itms = generateLevel(idSlug, 1, args.maxLevel || 1, args.maxChildrenCount || 0, args.minChildrenCount || 0);
    console.log("type of itms is: " + typeof(itms));
    let head = itms.filter(function (i) {
                  return i.hasPriorVersions;
                }).at(0)
    let idxArr = [...Array(n-1).keys()].map(function(v){
        return v + 1;
    }); //1st arr is (0 to n-1)

    let emailAdds = ["eferagh1i@slate.com",
        "brobins3@cdbaby.com",
        "sellerayl@epa.gov",
        "jvanhalen2c@webnode.com",
        "jcristofalos@indiatimes.com",
        "wshovell1f@squarespace.com",
        "gmacconnulty2r@about.com",
        "mcohen10@tinyurl.com"];

/* from commAux
		const arrFold = (fn, init, arr) => {
			return arr.reduce(fn, init);
		}
*/

    let foldr = function (prevVal, currVal, currIdx, arry){
        let newId = idSlug + currIdx;
        let newStr = "Edited by " + emailAdds(currIdx) + "\n" + head.string;
        let newVal = prevVal[0];
        newVal.id = [newId];
        newVal.string = [newStr];
				//@ToDo: assign a rnd later date/time
        return prevVal.push(newVal);
    };

    return arrFold(fldr, head, idxArr);
  }

	//For ref; gen via mJSONStringify(items.[2])
	let stringified2 =  {
		"id": "DocID_632294224800000000^usr2@brij.com^Table01^BrijCorp^3",
		"number": 8030,
		"string": "kx  3fyx hcczul 0 u2plf 2zc0 n xts 0 ",
		"date": "2037-09-29",
		"time": "T01:41:14",
		"bool": true,
		"hasPriorVersions": true,
		"isCateg": true
	};

	const getPriorItmsMap = () => {
		let priors = mGenPriors(8); //gets [8 priors]
		return new Map(
			priors.map(obj => {
				return [obj.id, obj];
			}),
		)};

	const getPriorItm = (id) => {
			(getPriorItmsMap).get(id);
	};

	return {
		getData: function(args){
			var data = {
				identifier: 'id', 
				label: 'id', 
				items: generateLevel('DocID_632294224800000000^usr2@brij.com^Table01^BrijCorp^', 1, args.maxLevel || 1, args.maxChildrenCount || 0, args.minChildrenCount || 0)
			};
			console.log("data:", data);
			console.log("stringified 2:", mJSONStringify(data.items[2]));
			return data;
		},
		
		layouts: [
			[
				{id: 'number', name: 'number', field: 'number', expandLevel: 1},
				{id: 'string', name: 'string', field: 'string', expandLevel: 2},
				{id: 'date', name: 'date', field: 'date', expandLevel: 3},
				{id: 'time', name: 'time', field: 'time'},
				{id: 'bool', name: 'bool', field: 'bool'},
				{id: 'id', name: 'id', field: 'id'},
				{id: 'hasPriorVersions', name: 'hasPriorVersions', field: 'hasPriorVersions'},
				{id: 'isCateg', name: 'isCateg', field: 'isCateg'}
			],
			[
				{id: 'number', name: 'number', field: 'number'},
				{id: 'string', name: 'string', field: 'string'},
				{id: 'date', name: 'date', field: 'date'},
				{id: 'time', name: 'time', field: 'time'},
				{id: 'bool', name: 'bool', field: 'bool'},
				{id: 'id', name: 'id', field: 'id'},
				{id: 'hasPriorVersions', name: 'hasPriorVersions', field: 'hasPriorVersions'},
				{id: 'isCateg', name: 'isCateg', field: 'isCateg'}
			]
		]
	};
});



