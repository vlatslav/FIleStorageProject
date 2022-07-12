import GetCategoryId from "./GetCategoryId";
import {variables} from "../Variables/Variables";
import {Button, Dropdown, Modal} from "react-bootstrap";
import React, {useState} from "react";
import './AddFile.css';

function AddFile(props) {

    const [selectedFile, setSelectedFile] = useState();
    const [categoryId, setcategoryId] = useState();
    const [titleValue, setTitleValue] = useState();
    const [descValue, setDescValue] = useState();
    const changeHandler = (event) => {
        setSelectedFile(event.target.files[0]);
    };
    const closeWindow = () => {

        if(titleValue === undefined && descValue === undefined && selectedFile === undefined) {
            props.handleClose();
        }
        else if(titleValue === undefined || descValue === undefined)
            alert("Please enter title and description for file")
        else {
            props.handleClose();
            setTitleValue(null);
            setSelectedFile(null);
            setDescValue(null);
            setcategoryId("SelectCategory");
        }
    }
    const handleSubmission = () => {
        if(categoryId !== "SelectCategory" && selectedFile !== null) {
            const formData = new FormData();
            formData.append('File', selectedFile);
            const token = 'Bearer ' + JSON.parse(localStorage.getItem('user'));
            console.log(token);
            fetch(
                variables.API_URL + "File/uploadfile/" + categoryId,
                {
                    method: 'POST',
                    body: formData,
                    headers: {
                        'Authorization': token
                    }
                }
            )
                .then((result) => {
                    console.log('Success:', result);
                    props.refreshPage();
                })
                .catch((error) => {
                    console.error('Error:', error);
                });
        }else{
            alert("You forgot to fill some fields.");
        }
    };
    const patchTextAndDesc =() => {
        if(titleValue !== null && descValue !== null) {
            props.handleClose();
            fetch(variables.API_URL + "File/" + props.files?.at(-1).fileId, {
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
                    props.refreshPage();
                })
        }else{
            alert("You forgot to fill some fields.");
        }
    }
    const setValueToCat = (text) => {
        const idout = GetCategoryId(text);
        setcategoryId(idout);
    };

    return (
        <>
            <Modal show={props.show} onHide={closeWindow}>
                <Modal.Header closeButton>
                    <Modal.Title>Add File</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div style={{width: '100%', height: '100%'}}>
                        <h6 style={{paddingBottom: '1%'}}>Select FileCategory</h6>

                        <Dropdown>
                            <Dropdown.Toggle variant="primary" id="dropdown-basic">
                                File Category
                            </Dropdown.Toggle>

                            <Dropdown.Menu>
                                <Dropdown.Item>
                                    <div
                                        onClick={(e) =>
                                            setValueToCat(e.target.textContent)}
                                    >Games
                                    </div>
                                </Dropdown.Item>
                                <Dropdown.Item>
                                    <div
                                        onClick={(e) =>
                                            setValueToCat(e.target.textContent)}
                                    >Images
                                    </div>
                                </Dropdown.Item>
                                <Dropdown.Item>
                                    <div
                                        onClick={(e) =>
                                            setValueToCat(e.target.textContent)}
                                    >Videos
                                    </div>
                                </Dropdown.Item>
                                <Dropdown.Item>
                                    <div
                                        onClick={(e) =>
                                            setValueToCat(e.target.textContent)}

                                    >Books
                                    </div>
                                </Dropdown.Item>
                                <Dropdown.Item>
                                    <div
                                        onClick={(e) =>
                                            setValueToCat(e.target.textContent)}
                                    >Scripts
                                    </div>
                                </Dropdown.Item>
                            </Dropdown.Menu>
                        </Dropdown>
                        <h6 style={{paddingTop: '2%'}}>Select file from your computer</h6>
                        <input style={{paddingTop: '1%', paddingBottom: '2%'}} type="file" name="file" onChange={changeHandler} />
                        <Button variant="primary" onClick={handleSubmission}>
                            UploadFile
                        </Button>
                        <h6>Enter FileName</h6>
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
export default AddFile;