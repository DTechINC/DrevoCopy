function openAdd(parentId) {
    $("#isList").val(false);
    $('#parentId').val(parentId);
    $('#overlay').show();
    $('#title').focus();
}

function checkAndAdd() {
    $('#title').val() ? $('#AddForm').submit() :  $('#title').focus();
}

function expand(elem) {
    var li = $(elem).parents('li')[0];
    var ul = $(li).children('ul')[0];
    $(ul).css('display') == 'none' ? $(ul).show() : $(ul).hide();
}

function selectList()
{
    if ($("#cbList").attr("checked") == 'checked') {
        $('#isList').val(true);
    }
    else {
        $('#isList').val(false);
    }
}