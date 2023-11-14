function VU_RemoveDomain (LinkSting) {
    if (LinkSting != '') {
        //let rx = new RegExp('((([a-z\\d]([a-z\\d-]*[a-z\\d])*)\\.)+[a-z]{2,}|((\\d{1,3}\\.){3}\\d{1,3}))');
        //let StrDomain = rx.exec(LinkSting) != null ? rx.exec(LinkSting)[0] : '';
        if (LinkSting.indexOf('.com') != -1) {
            LinkSting = LinkSting.slice(LinkSting.lastIndexOf('.com/') + 5, LinkSting.length);
        }
        if (LinkSting.indexOf('.me') != -1) {
            LinkSting = LinkSting.slice(LinkSting.lastIndexOf('.me/') + 4, LinkSting.length);
        }
        return LinkSting;
    } else {
        return LinkSting;
    }
}