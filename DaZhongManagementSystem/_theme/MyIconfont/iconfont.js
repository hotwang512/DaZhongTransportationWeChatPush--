;(function(window) {

  var svgSprite = '<svg>' +
    '' +
    '<symbol id="icon-suiji" viewBox="0 0 1448 1024">' +
    '' +
    '<path d="M972.634861 262.65545h198.614165c27.468136 0 49.660422-22.242172 49.660422-49.660422 0-13.863083-7.365892-26.312162-14.711141-35.303642l1.837172 1.835452s-2.024674-2.729956-5.554522-5.812552a151.60801 151.60801 0 0 0-10.63771-10.37968L1040.415858 11.908937h-140.505846l151.425669 151.425669H972.634861c-191.955275 0-347.547265 155.616073-347.547265 347.571348 0 136.881387-111.370837 248.250504-248.276307 248.250503h-148.955463v99.319124h148.955463c191.955275 0 347.571348-155.616073 347.571348-347.571348 0-136.877947 111.370837-248.248784 248.252224-248.248783z" fill="" ></path>' +
    '' +
    '<path d="M376.811289 163.336326h-148.955463v99.320844h148.955463c93.83685 0 174.892622 52.718936 216.925682 129.797608 11.578659-36.998038 28.501976-71.572316 49.823841-103.038194-63.709286-76.774197-159.192367-126.080258-266.749523-126.080258zM1220.909448 808.816879c0-27.41653-22.192286-49.658702-49.660422-49.658701H972.634861c-93.742239 0-174.725763-52.577879-216.762263-129.489693a394.408924 394.408924 0 0 1-49.705147 103.08464c63.709286 76.512727 159.073673 125.725897 266.46741 125.725897h78.70254l-151.425669 151.425668h140.505846l151.427389-151.425668s5.01266-4.331461 10.63771-10.37796c3.531568-3.130762 5.554522-5.814272 5.554522-5.814273l-1.837172 1.835453c7.297084-8.99148 14.709421-21.464642 14.709421-35.305363z" fill="" ></path>' +
    '' +
    '</symbol>' +
    '' +
    '<symbol id="icon-xuanze" viewBox="0 0 1024 1024">' +
    '' +
    '<path d="M455.943579 766.249018L276.117858 552.721591l51.254578-52.646574 128.571143 103.273188 240.727334-218.466844 51.252772 52.64537-291.980106 328.722287z m336.224132-599.844736c0.054789 0.052983-18.555325 16.348121-18.505956 16.455291v460.799307c-0.04937 0.052983 0.107771 197.420136 0 197.483354H250.308141c-0.056595-0.063218 0.056595-131.552012 0-131.659784v-394.966706c0.056595-0.059605-0.100546-131.594759 0-131.656171H119.466576l0.037329 674.736747c0 63.369515 50.459238 115.203891 112.150773 115.203891h560.73279c61.684912 0 112.145956-51.834375 112.145956-115.203891l-0.0289-674.736747-112.336813-16.455291z m-149.34752 16.455291V117.034197c0-31.788314-43.783439-65.833805-74.725839-65.833806H455.943579c-30.937584 0-74.799292 34.046093-74.799292 65.833806v65.825376H315.725913v131.656171h392.517468V182.859573h-65.42319z m-65.520125 0.44433l-130.732589-0.44433V117.034197h130.732589v66.269706z"  ></path>' +
    '' +
    '</symbol>' +
    '' +
    '</svg>'
  var script = function() {
    var scripts = document.getElementsByTagName('script')
    return scripts[scripts.length - 1]
  }()
  var shouldInjectCss = script.getAttribute("data-injectcss")

  /**
   * document ready
   */
  var ready = function(fn) {
    if (document.addEventListener) {
      if (~["complete", "loaded", "interactive"].indexOf(document.readyState)) {
        setTimeout(fn, 0)
      } else {
        var loadFn = function() {
          document.removeEventListener("DOMContentLoaded", loadFn, false)
          fn()
        }
        document.addEventListener("DOMContentLoaded", loadFn, false)
      }
    } else if (document.attachEvent) {
      IEContentLoaded(window, fn)
    }

    function IEContentLoaded(w, fn) {
      var d = w.document,
        done = false,
        // only fire once
        init = function() {
          if (!done) {
            done = true
            fn()
          }
        }
        // polling for no errors
      var polling = function() {
        try {
          // throws errors until after ondocumentready
          d.documentElement.doScroll('left')
        } catch (e) {
          setTimeout(polling, 50)
          return
        }
        // no errors, fire

        init()
      };

      polling()
        // trying to always fire before onload
      d.onreadystatechange = function() {
        if (d.readyState == 'complete') {
          d.onreadystatechange = null
          init()
        }
      }
    }
  }

  /**
   * Insert el before target
   *
   * @param {Element} el
   * @param {Element} target
   */

  var before = function(el, target) {
    target.parentNode.insertBefore(el, target)
  }

  /**
   * Prepend el to target
   *
   * @param {Element} el
   * @param {Element} target
   */

  var prepend = function(el, target) {
    if (target.firstChild) {
      before(el, target.firstChild)
    } else {
      target.appendChild(el)
    }
  }

  function appendSvg() {
    var div, svg

    div = document.createElement('div')
    div.innerHTML = svgSprite
    svgSprite = null
    svg = div.getElementsByTagName('svg')[0]
    if (svg) {
      svg.setAttribute('aria-hidden', 'true')
      svg.style.position = 'absolute'
      svg.style.width = 0
      svg.style.height = 0
      svg.style.overflow = 'hidden'
      prepend(svg, document.body)
    }
  }

  if (shouldInjectCss && !window.__iconfont__svg__cssinject__) {
    window.__iconfont__svg__cssinject__ = true
    try {
      document.write("<style>.svgfont {display: inline-block;width: 1em;height: 1em;fill: currentColor;vertical-align: -0.1em;font-size:16px;}</style>");
    } catch (e) {
      console && console.log(e)
    }
  }

  ready(appendSvg)


})(window)