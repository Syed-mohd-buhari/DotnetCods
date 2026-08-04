import React from 'react'


interface Props {
    children: any
    show: boolean
}


const Container: React.FC<Props> = (props) => {
    return (
        <>
            {props.show === true ? props.children : null}
        </>
    )
}

export default Container
