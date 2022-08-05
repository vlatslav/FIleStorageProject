import {variables} from "../Variables/Variables"
import {Button, Modal} from "react-bootstrap";
import React, {useEffect, useState} from "react";
import axios from "axios";


function AddTitle(props) {

    const [titleValue, setTitleValue] = useState();
    const [descValue, setDescValue] = useState();
    const [posts, setPosts] = useState();
    useEffect(() => {
        const fetchPosts = async () => {
            const res = await axios.get('https://localhost:5001/api/File/files');
            setPosts(res.data);
            props.handlerOff();
        };
        fetchPosts();
    }, [props.handler]);
    const closeWindow = () => {
        if(titleValue !== undefined && descValue !== undefined) {
            props.handleClose();
            setTitleValue(undefined);
            setDescValue(undefined);
        }else{
            alert("You have to fill all fields");
        }
    }
    const patchTextAndDesc =() => {
        if(titleValue !== undefined && descValue !== undefined) {
            props.handleClose();
            fetch(variables.API_URL + "File/" + posts?.at(-1).fileId, {
                method: 'PATCH',
                body: JSON.stringify([
                    {
                        op: "replace",
                        path: "Title",
                        value: titleValue
                    },
                    {
                        op: "replace",
                        path: "Description",
                        value: descValue
                    }
                ]),
                headers: {
                    'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('user')),
                    'Content-Type': 'application/json'
                },
            })
                .then((response) => {
                    if(!response.ok){
                        throw new Error();
                    }
                    props.refresh();
                }).catch((error) => {
                console.error('Error:', error);
            });
        }else{
            alert("You forgot to fill some fields.");
        }
    }
    return (
        <>
            <Modal show={props.show} onHide={closeWindow}>
                <Modal.Header closeButton>
                    <Modal.Title>Add Title and Description for file</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div style={{width: '100%', height: '100%'}}>
                        <h6>Enter File name</h6>
                        <input type="text" className="form-control"
                               onChange={(e) =>
                                   setTitleValue(e.target.value)}
                        />
                        <h6>Enter File description</h6>
                        <input type="text" className="form-control"
                               onChange={(e) =>
                                   setDescValue(e.target.value)}
                        />

                    </div>
                </Modal.Body>
                <div style={{paddingLeft: '3%', paddingBottom: '2%'}}>
                    <Button variant="secondary" onClick={closeWindow}>
                        Close
                    </Button>
                    <Button variant="primary" onClick={patchTextAndDesc}>
                        Submit
                    </Button>
                </div>
            </Modal>

        </>

    );
}
export default AddTitle;