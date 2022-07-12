import React, {useState} from "react";
import {Button, Dropdown, Modal} from "react-bootstrap";
import {variables} from "../Variables/Variables";
import GetCategoryId from "../AddFile/GetCategoryId";

function UpdateFile(props) {

    const [dropDownValue, setDropDownValue] = useState("SelectCategory");

    const [titleValue, setTitleValue] = useState();
    const [descValue, setDescValue] = useState();


    const patchTextAndDesc =() => {
        const index = GetCategoryId(dropDownValue);
        fetch(variables.API_URL + "File/" + props.currentFile.fileId, {
            method: 'PATCH',
            body: JSON.stringify([
                {
                    op:"replace",
                    path: "Title",
                    value: titleValue
                },
                {
                    op:"replace",
                    path:"Description",
                    value: descValue
                },
                {
                    op:"replace",
                    path: "CategoryId",
                    value: index
                }
            ]),
            headers: {
                'Content-type': 'application/json; charset=UTF-8',
                'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('user')),
            },
        })
            .then((response) => {
                console.log(response);
                props.refreshPage();
            })
    }
    const changeValue = (text) => {
        setDropDownValue(text);
    }
    const closeWindow = () => {
        props.handleCloseEdit();
        setDescValue(null);
        setTitleValue(null);
        setDropDownValue("SelectCategory");
    }

    const updateFileClick = () => {
        props.handleCloseEdit()
        if (titleValue !== null && descValue !== null && dropDownValue !== "SelectCategory") {
            patchTextAndDesc();
        } else {
            alert("You forgot to fill some fields.")
        }

    }
    if(props.currentFile !== undefined) {
        return (
            <>

                <Modal show={props.showEdit} onHide={props.handleCloseEdit}>
                    <Modal.Header closeButton>
                        <Modal.Title>UpdateBook</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <div>
                            <h5>Selected File Id= {props.currentFile.fileId}</h5>
                            <h5>Enter FileTitle</h5>
                            <input type="text" className="form-control"
                                   onChange={(e) =>
                                       setTitleValue(e.target.value)}
                            />
                            <h5>Enter File Description</h5>
                            <input type="text" className="form-control"
                                   onChange={(e) =>
                                       setDescValue(e.target.value)}
                            />
                            <h5>Select FileCategory</h5>

                            <Dropdown>
                                <Dropdown.Toggle variant="success" id="dropdown-basic">
                                    {dropDownValue}
                                </Dropdown.Toggle>

                                <Dropdown.Menu>
                                    <Dropdown.Item>
                                        <div
                                            onClick={(e) =>
                                                changeValue(e.target.textContent)}
                                        >Games
                                        </div>
                                    </Dropdown.Item>
                                    <Dropdown.Item>
                                        <div
                                            onClick={(e) =>
                                                changeValue(e.target.textContent)}

                                        >Images
                                        </div>
                                    </Dropdown.Item>
                                    <Dropdown.Item>
                                        <div
                                            onClick={(e) =>
                                                changeValue(e.target.textContent)}
                                        >Videos
                                        </div>
                                    </Dropdown.Item>
                                    <Dropdown.Item>
                                        <div
                                            onClick={(e) =>
                                                changeValue(e.target.textContent)}
                                        >Books
                                        </div>
                                    </Dropdown.Item>
                                    <Dropdown.Item>
                                        <div
                                            onClick={(e) =>
                                                changeValue(e.target.textContent)}
                                        >Scripts
                                        </div>
                                    </Dropdown.Item>
                                </Dropdown.Menu>
                            </Dropdown>
                        </div>
                    </Modal.Body>
                    <Modal.Footer>
                        <Button variant="secondary" onClick={

                            closeWindow

                        }>
                            Close
                        </Button>
                        <Button variant="primary" onClick={updateFileClick}>
                            UpdateFile
                        </Button>
                    </Modal.Footer>
                </Modal>


            </>

        )
    }
    else return (<></>)

}

export default UpdateFile;