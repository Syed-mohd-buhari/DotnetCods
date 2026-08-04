import { rootStore } from "../Store/rootStore";

export default function setLoader(show:boolean = false) {
if(show){
    rootStore.dispatch({ type: "SHOW_LOADER", show: true })
}else
{
    rootStore.dispatch({ type: "HIDE_LOADER", show:false })
}
}
