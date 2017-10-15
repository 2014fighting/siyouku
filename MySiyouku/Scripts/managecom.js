(function ($) {
    window.wenqing = {};//全局对象
    wenqing.tabadd = function(title, url) {
        var randomNum = Math.random(),//id
        k = true;
        if (title == undefined || $.trim(url).length === 0) {
            return false;
        };
 
        parent.$(".J_menuTab").each(function (x) {
          
            if ($(this).data("id") == url) {
                if (!$(this).hasClass("active")) {
                    $(this).addClass("active").siblings(".J_menuTab").removeClass("active");
                    console.info(parent.$(".J_mainContent .J_iframe"));
                    parent.$(".J_mainContent .J_iframe").each(function (i) {
                        if ($(this).data("id") == url) {
                            $(this).show().siblings(".J_iframe").hide();
                            return false;
                        }
                    });
                }
                k = false;
                return false;
            }
        });
        if (k) {
            var p = '<a href="javascript:;" class="active J_menuTab" data-id="'+url+'">' + title + ' <i class="fa fa-times-circle"></i></a>';
            parent.$(".J_menuTab").removeClass("active");
            var n = '<iframe class="J_iframe" name="iframe1' + randomNum + '" width="100%" height="100%" ' +
                'src="'+url+'" frameborder="0" data-id="' + url + '" seamless></iframe>';
            parent.$(".J_mainContent").find("iframe.J_iframe").hide().parents(".J_mainContent").append(n);
            parent.$(".J_menuTabs .page-tabs-content").append(p);
        }
        return false;
    };

    wenqing.openDialog = function (fromSrc, title, width, height) {
  
        var obj = '<div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">' +
            '<div  class="modal-dialog" style="width:'+width+'px;"  role="document">' +
            '<div class="modal-content" >' +
            '<div class="modal-header">' +
            '<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>' +
            '<h4 class="modal-title" id="myModalLabel">' + title + '</h4>' +
            '</div>' +
     
            '<iframe id="myframe" src=' + fromSrc + ' width="' + (width-30) + '" height="'+height+'" frameborder="0"></iframe>' +
           
            '</div>' +
            '</div>' +
            '</div>';
         $(obj).modal("show");
        
    

    }

    //url1 要关闭的url     url2 要定位的url
    wenqing.tabclose = function (url1, url2) {
 
        if (url1 == undefined || url2 == undefined) {
            return false;
        };
        parent.$("a[data-id*='" + url1 + "']").remove();
        parent.$(".J_menuTab").each(function () {
            if ($(this).data("id") == url2) {
                if (!$(this).hasClass("active")) {
                    $(this).addClass("active").siblings(".J_menuTab").removeClass("active");
                    parent.$(".J_mainContent .J_iframe").each(function () {
                        var x = $(this).data("id");
                        if ($(this).data("id") == url2) {
                            //console.info($(this).contents().find("[name=refresh]"));
                            //获取当前iframe 的datagrid 进行刷新
                            var tempelm = $(this).contents().find("[name=refresh]");
                            if(tempelm!=undefined)
                               $(this).contents().find("[name=refresh]").click();

                            $(this).show().siblings(".J_iframe").hide();
                        } else if ($(this).data("id") == url1) {
                            $(this).remove();
                        }
                    });
                }
                return false;
            } 
 
        });
        
        return false;
    };

    wenqing.delete = function(url,gridid) {
        var selRows = $('#'+gridid).bootstrapTable('getSelections');
        if (selRows.length == 0) {
            layer.msg('请选择要删除的数据!');
            return;
        };
        layer.confirm('确定要删除选中的数据？？', {
            btn: ['是滴！', '不点错了！'] //按钮
        }, function () {
            var postdata = [];
            for (var i = 0; i < selRows.length; i++) {
                postdata.push(selRows[i].Id);
            };
            $.ajax({
                type: "post",
                url: url,
                contentType: "application/json;charset=UTF-8",
                beforeSend: function () {//等待效果
                    //CreateWaitDiv();
                },
                complete: function () {//去除等待效果
                    //RemoveWaitDiv();
                },
                data: JSON.stringify(postdata),
                success: function (data) {
                    data = JSON.parse(data);
                    if (data.Code===0) {
                        layer.msg("删除成功！!");
                        $('#'+gridid).bootstrapTable('refresh');
                    } else {
                        layer.msg(data.Msg);
                    }
                },
                error: function (xhr, a, b) { alert(xhr.responseText); }
            });
        }, function () {
            //点错了
        });
    };

    wenqing.add = function (/*表单id*/formid,/*请求地址*/url,/*要关闭的url*/url1,/*要定位的url*/url2) {
        $.post(url,
            $('#' + formid).serializeArray(),
            function (data) {
                console.info(data);
                data = JSON.parse(data);
                if (data.Code === 0) {
                    layer.confirm('保存数据成功!是否返回列表页面？', {
                        btn: ['是', '否'] //按钮
                    }, function () {
                        if (url1 && url2)
                            wenqing.tabclose(url1, url2);
                    }, function () {
                        document.getElementById(formid).reset();
                    });
                     
                   
                }
                else
                    layer.msg("保存失败：" + data.Msg);
            });
    }

    wenqing.edit = function (/*表单id*/formid,/*请求地址*/url,/*要关闭的url*/url1,/*要定位的url*/url2) {
        $.post(url,
            $('#' + formid).serializeArray(),
            function (data) {
                data = JSON.parse(data);
                if (data.Code === 0)
                    layer.confirm('保存数据成功!是否返回列表页面？', {
                        btn: ['是', '否'] //按钮
                    }, function () {
                        if (url1 && url2) {
                            wenqing.tabclose(url1, url2);
                        }
                    }, function () {
                        //document.getElementById(formid).reset();
                    });
                else
                    layer.msg("出错了：" + data.Mag);
            });
    }

    wenqing.getHeight = function () { return $(window).height() - $('h1').outerHeight(true); };
})(jQuery)