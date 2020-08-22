function copyToClipboard(id) {

    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val("http://localhost:63810/api" + $("#" + id).attr("href")).select();
    document.execCommand("copy");
    $temp.remove();

}