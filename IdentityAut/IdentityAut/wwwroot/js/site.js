function readCookie(name) {
        var matches = document.cookie.match(new RegExp(
    "(?:^|; )" + name.replace(/([\.$?*|{ }\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
        ));

    return matches ? decodeURIComponent(matches[1]) : undefined;
}
function GetCookieName() {
    let el = document.getElementById("NameField");
    el.textContent = readCookie("name");
}


function GetCookieTheme() {

    Classtheme = document.getElementById("theme");

    if (readCookie("theme") == "Dark") {

        document.body.style.background = '#2b2b2b';
        Classtheme.style.background = '#2b2b2b';

    }
}
