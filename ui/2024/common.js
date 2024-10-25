//this is the new version (includes init)
const arrFold = (fn, init, arr) => {
  return arr.reduce(fn, init);
}

/* min (inclusive) and max (exclusive)*/
const rNext = (min, max) => {
  return parseInt(((Math.random() * (max - min)) + min));
}


/*                Cookies + ipInfo
*/
const setCookie = (nm, val, days) => {
  const d = new Date();
  d.setTime(d.getTime() + (days * 24 * 60 * 60 * 1000));
  let expires = "expires="+d.toUTCString();
  document.cookie = nm + "=" + val + ";" + expires + ";path=/";
}

const getCookie = (strKey) => {
  let name = strKey + "=";
  let ca = document.cookie.split(';');
  for(let i = 0; i < ca.length; i++) {
    let c = ca[i];
    while (c.charAt(0) == ' ') {
      c = c.substring(1);
    }
    if (c.indexOf(name) == 0) {
      return c.substring(name.length, c.length);
    }
  }
  return "";
}

const checkCookie = () => {
  let user = getCookie("username");
  if (user != "") {
    alert("Welcome again " + user);
  } else {
    user = prompt("Please enter your name:", "");
    if (user != "" && user != null) {
      setCookie("username", user, 30);
    }
  }
}

const usrDetails = async () => {
  try{
    const res = await fetch("http://ip-api.com/line/?fields=9264");
    const t = await res.text();
    return ("Success:", t);
  } catch (error) {
    return ("Error:", error);
  }
      //.then(data => console.log(data));
}

const gfxRun = async () => {
  try{
    //const res = await fetch("http://ip-api.com/line/?fields=9264");
    //const t = await res.text();
    const gfxTestData = [
        {id:0, type: 'type1', name:"One", annex: "L-shaped tree"},
        {id:1, type: 'type2', name:"Two", biscuit: false},
        {id:2, type: 'type2', name:"Three", biscuit: true}
    ];
    return ("Success:", gfxTestData);
  } catch (error) {
    return ("Error:", error);
  }
      //.then(data => console.log(data));
}

/*
require(["dojo/store/Memory"], function(Memory){
    var someData = [{id:1, name:"One"},
                  {id:2, name:"Two"}];
    store = new Memory({data: someData});
    store.get(1) -> Returns the object with an id of 1
    store.put({id:3, name:"Three"}); // store the object with the given identity
    store.remove(3); // delete the object
});

*/
/*                Cookies + ipInfo
*/


/*                Mock Fetch Calls
see: https://stackoverflow.com/questions/45425169/intercept-fetch-api-requests-and-responses-in-javascript
*/
const {fetch: origFetch} = window;
window.fetch = async (...args) => {
  console.log("fetch called with args:", args);
  const response = await origFetch(...args);

  /* work with the cloned response in a separate promise
     chain -- could use the same chain with `await`. */
  response
    .clone()
    .json()
    .then(data => console.log("intercepted response data:", data))
    .catch(err => console.error(err));

  //return response;   /* original response */

  /* mocked response: */
  return new Response(JSON.stringify({
    userId: 1,
    id: 1,
    title: "Mocked!!",
    completed: false
  }));
};

const testFetchCall = () => {
   fetch("https://jsonplaceholder.typicode.com/todos/1")
     .then(response => response.json())
     .then(data => console.log("original caller received:", data))
     .catch(err => console.error(err));
}

