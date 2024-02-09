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


