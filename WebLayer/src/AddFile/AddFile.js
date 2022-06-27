import GetCategoryId from "./GetCategoryId";
import {variables} from "../Variables/Variables";
import {Button, Dropdown, Modal} from "react-bootstrap";
import React, {useState} from "react";

function AddFile(props) {

    const [selectedFile, setSelectedFile] = useState();
    const [categoryId, setcategoryId] = useState();
    const [titleValue, setTitleValue] = useState();
    const [descValue, setDescValue] = useState();
    const changeHandler = (event) => {
        setSelectedFile(event.target.files[0]);
    };
    const closeWindow = () => {
        props.handleClose();
        setTitleValue(null);
        setSelectedFile(null);
        setDescValue(null);
        setcategoryId("SelectCategory");
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
                    'Authentication': 'Bearer ' + JSON.parse(localStorage.getItem('user')),
                    'Content-Type': 'application/json'
                },
            })
                .then((response) => {
                    console.log(response);
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
    console.log(localStorage.getItem('UserId'));

    return (
        <>
            <Modal show={props.show} onHide={props.handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>Add File</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div>
                        <h5>Select FileCategory</h5>

                        <Dropdown>
                            <Dropdown.Toggle variant="success" id="dropdown-basic">
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
                        <input type="file" name="file" onChange={changeHandler} />
                        <button variant="primary" onClick={handleSubmission}>
                            UploadFile
                        </button>
                        <h5>Enter FileName</h5>
                        <input type="text" className="form-control"
                               onChange={(e) =>
                                   setTitleValue(e.target.value)}
                        />
                        <h5>Enter File description</h5>
                        <input type="text" className="form-control"
                               onChange={(e) =>
                                   setDescValue(e.target.value)}
                        />

                    </div>
                </Modal.Body>
                <div>
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