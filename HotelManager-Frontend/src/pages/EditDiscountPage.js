import React from "react";
import { NavBar } from "../components/Common/NavBar";
import { useParams } from "react-router-dom";
import DiscountEdit from "../components/discount/DiscountEdit";

export const EditDiscountPage = () =>{
    const { discountId } = useParams();
    return(
        <div>
            <NavBar/>
            <DiscountEdit discountId={discountId}/>
        </div>
    )
}