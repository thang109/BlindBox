<script data-n-head="ssr" type="text/javascript" id="vwoCode" async>
    window._vwo_code || function() {
            var i = 869229,
    n = !1,
    r = window,
    s = document,
    o = s.querySelector("#vwoCode"),
    a = "_vwo_" + i + "_settings",
    e = { };
    try {
                var t = JSON.parse(localStorage.getItem("_vwo_" + i + "_config")),
    e = t && "object" == typeof t ? t : { }
            } catch (e) { }
    var d = "session" === e.stT ? r.sessionStorage : r.localStorage;
    code = {
        use_existing_jquery: function() {
                    return "undefined" != typeof use_existing_jquery ? use_existing_jquery : void 0
                },
    library_tolerance: function() {
                    return "undefined" != typeof library_tolerance ? library_tolerance : void 0
                },
    settings_tolerance: function() {
                    return e.sT || 2e3
                },
    hide_element_style: function() {
                    return "{" + (e.hES || "opacity:0 !important;filter:alpha(opacity=0) !important;background:none !important") + "}"
                },
    hide_element: function() {
                    return performance.getEntriesByName("first-contentful-paint")[0] ? "" : "string" == typeof e.hE ? e.hE : "body"
                },
    getVersion: function() {
                    return 2.1
                },
    finish: function(e) {
                    var t;
    n || (n = !0, (t = s.getElementById("_vis_opt_path_hides")) && t.parentNode.removeChild(t), e && ((new Image).src = "../dev.visualwebsiteoptimizer.com/ee5ba7.gif?a=" + i + e))
                },
    finished: function() {
                    return n
                },
    addScript: function(e) {
                    var t = s.createElement("script");
    t.type = "text/javascript", e.src ? t.src = e.src : t.text = e.text, s.getElementsByTagName("head")[0].appendChild(t)
                },
    load: function(e, t) {
                    var n, i = this.getSettings(),
    o = s.createElement("script");
    t = t || { }, i ? (o.textContent = i, s.getElementsByTagName("head")[0].appendChild(o), r.VWO && !VWO.caE || (d.removeItem(a), this.load(e))) : ((n = new XMLHttpRequest).open("404.html", e, !0), n.withCredentials = !t.dSC, n.responseType = t.responseType || "text", n.onload = function() {
                        if (t.onloadCb) return t.onloadCb(n, e);
    200 === n.status ? _vwo_code.addScript({
        text: n.responseText
                        }) : _vwo_code.finish("&e=loading_failure:" + e)
                    }, n.onerror = function() {
                        if (t.onerrorCb) return t.onerrorCb(e);
    _vwo_code.finish("&e=loading_failure:" + e)
                    }, n.send())
                },
    getSettings: function() {
                    try {
                        var e = d.getItem(a);
    if (e) {
                            if (e = JSON.parse(e), !(Date.now() > e.e)) return e.s;
    d.removeItem(a)
                        }
                    } catch (e) { }
                },
    init: function() {
                    var e, t, n; - 1 < s.URL.indexOf("__vwo_disable__") || (n = this.settings_tolerance(), r._vwo_settings_timer = setTimeout(function() {
        _vwo_code.finish(), d.removeItem(a)
    }, n), "body" !== this.hide_element() ? (e = s.createElement("style"), t = (n = this.hide_element()) ? n + this.hide_element_style() : "", n = s.getElementsByTagName("head")[0], e.setAttribute("id", "_vis_opt_path_hides"), o && e.setAttribute("nonce", o.nonce), e.setAttribute("type", "text/css"), e.styleSheet ? e.styleSheet.cssText = t : e.appendChild(s.createTextNode(t)), n.appendChild(e)) : (e = s.getElementsByTagName("head")[0], (t = s.createElement("div")).style.cssText = "z-index: 2147483647 !important;position: fixed !important;left: 0 !important;top: 0 !important;width: 100% !important;height: 100% !important;background: white !important;", t.setAttribute("id", "_vis_opt_path_hides"), t.classList.add("_vis_hide_layer"), e.parentNode.insertBefore(t, e.nextSibling)), n = "https://dev.visualwebsiteoptimizer.com/j.php?a=" + i + "&u=" + encodeURIComponent(s.URL) + "&vn=2.1", -1 !== r.location.search.indexOf("_vwo_xhr") ? this.addScript({
        src: n
                    }) : this.load(n + "&x=true"))
                }
            }, (r._vwo_code = code).init()
        }()
</script>
