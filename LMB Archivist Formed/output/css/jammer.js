var jammers = document.getElementsByClassName("lia-spoiler-link");
var targets = document.getElementsByClassName("lia-spoiler-content");

var jamToggle = function (subJammer, subTarget) {
    var classListJammer = subJammer.classList;

    if (classListJammer.contains("open")) {
        classListJammer.remove("open");
        subTarget.style.cssText = '';
    }
    else {
        classListJammer.add("open");
        subTarget.style.cssText = 'display:block;';
    }
}

for (var i = 0; i < jammers.length; i++) {
    var jammer = jammers[i];
    jammer.onclick = (function () {
        var subJammer = jammers[i];
        var subTarget = targets[i];

        subJammer.removeAttribute("href");

        return function () {
            jamToggle(subJammer, subTarget);
        }
    })();
}