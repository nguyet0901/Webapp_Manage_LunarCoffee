﻿//#region //Handle Static Control

async function guidemo_stcontrol () {
    let data = BlockControl["Static"];
    if (data && data.length > 0) {
        for (let j = 0; j < data.length; j++) {
            let ele = data[j];
            guidemo_hideStaticElement(ele);
        }
    }
}

async function guidemo_hideStaticElement (ele) {
    await new Promise((resolve, reject) => {
        let {Selector, ParrentSelector, IsDisable} = ele;
        if (Selector !== undefined && Selector !== '') {
            let element = $(`${Selector}`);
            if (ParrentSelector !== undefined && ParrentSelector !== '') {
                element = element.closest(`${ParrentSelector}`);
            }
            if (IsDisable) {
                element.css({"cssText": "display: none !important"});
            }
            else {
                element.remove();
            }
            resolve();
        }
    })
}
//#endregion

//#region //Handle Dynamic Control
async function guidemo_dycontrol () {
    let data = BlockControl["Dynamic"];
    for (let i = 0; i < data.length; i++) {
        let item = data[i];
        let { MutationParent, ChildNode, IsMutation } = item;
        if (ChildNode && ChildNode.length > 0) {
            for (let j = 0; j < ChildNode.length; j++) {
                let ele = ChildNode[j];
                guidemo_hideDynamicElement(ele, MutationParent, IsMutation);
            }
        }
    }
}

function guidemo_hideDynamicElement(ele, mutationParent, isMutation) {
    //AttrMuta ['style']
    let { Selector, IsDisable, AttrMuta } = ele;
    if (Selector !== undefined && Selector !== '') {
        guidemo_findElementMuta(ele).then((e) => {
            if (IsDisable) {
                e.css({"cssText": "display: none !important"});
            }
            else {
                e.remove();
            }
            if (isMutation) {
                AttrMuta = AttrMuta ?? null;
                guidemo_hideElementMuta(ele, mutationParent, IsDisable, AttrMuta);
            }
        });
    }
}
//#endregion

//#region //Handle MutationObverse
async function guidemo_findElementMuta (ele, mutationParent) {
    return new Promise(resolve => {
        let {Selector, ParrentSelector} = ele;
        if (document.querySelector(Selector)) {
            let element = $(Selector);
            if (ParrentSelector !== undefined && ParrentSelector !== '') {
                element = element.closest(`${ParrentSelector}`);
            }
            return resolve(element);
        }

        const observer = new MutationObserver(mutations => {
            if (document.querySelector(Selector)) {
                let element = $(Selector);
                if (ParrentSelector !== undefined && ParrentSelector !== '') {
                    element = element.closest(`${ParrentSelector}`);
                }
                resolve(element);
                observer.disconnect();
            }
        });
        let mutationEle = mutationParent ? document.getElementById(mutationParent) : document.getElementsByTagName('main')[0];
        observer.observe(mutationEle, {childList: true, subtree: true});
    });
}

async function guidemo_hideElementMuta (ele, mutationParent, IsDisable, AttrMuta) {
    await new Promise((resolve, reject) => {
        const observer = new MutationObserver(mutations => {
            guidemo_findElementMuta(ele, mutationParent).then((e) => {
                if (IsDisable) {
                    e.css({"cssText": "display: none !important"});
                }
                else {
                    e.remove();
                }
                resolve();
            });
        });
        let option = (AttrMuta == null) ? { childList: true, subtree: true } : { childList: true, subtree: true, attributeFilter: AttrMuta };
        let mutationEle = (mutationParent != undefined) ? document.getElementById(mutationParent) : document.getElementsByTagName('main')[0];
        observer.observe(mutationEle, option);
    })
}

async function guidemo_mutabody () {
    let mutationParent = '', options = {}, isDisconect = true;

    mutationParent = 'MainRenderBody_Manual';
    isDisconect = false;
    options = {childList: false, subtree: false, attributes: true, attributeFilter: ['style']}

    const observer = new MutationObserver(mutations => {
        guidemo_stcontrol();
        guidemo_dycontrol();
        if (isDisconect) observer.disconnect();
    });
    observer.observe(document.getElementById(mutationParent), options);
}
    //#endregion