(function($) {
    window.wqobj = {};
    $(function () {

        $(".login_out").click(function () {
            layer.alert('功能尚未开放！');
        });
        $('.gotop').css('opacity', 0);
        $(window).on('scroll',
            function () {
                var scrollTop = $(window).scrollTop();
                if (scrollTop > 500) {
                    $('.gotop').css('opacity', 1);
                } else {
                    $('.gotop').css('opacity', 0);
                }
            });

        var speed = 600;//滚动速度
        var h = document.body.clientHeight;
        $(".gotop").click(function () {

            $('html,body').animate({

                scrollTop: '0px'
            },
          speed);
        });

    });


    wqobj.loging=function() {
       layer.msg('数据加载中...', {
           icon: 16
  , shade: 0.01
       });
   }


   wqobj.isPc = function () {
       var userAgentInfo = navigator.userAgent;
       var agents = ["Android", "iPhone",
                   "SymbianOS", "Windows Phone",
                   "iPad", "iPod"];
       var flag = true;
       for (var v = 0; v < agents.length; v++) {
           if (userAgentInfo.indexOf(agents[v]) > 0) {
               flag = false;
               break;
           }
       }
       return flag;
   }
   wqobj.isWeiXin=function () {
       var ua = window.navigator.userAgent.toLowerCase();
       if (ua.match(/MicroMessenger/i) == 'micromessenger') {
           return true;
       } else {
           return false;
       }
   }
})(jQuery);