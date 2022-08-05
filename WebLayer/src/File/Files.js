import React, { useState, useEffect } from 'react';
import axios from 'axios';
import Pagination from "./Pagination";
import Posts from "./Posts";
import {variables} from "../Variables/Variables";

const Files = () => {
    const [posts, setPosts] = useState([]);
    const [page, setPage] = useState();
    const [files, setFiles] = useState([]);
    const [loading, setLoading] = useState(false);
    const [currentPage, setCurrentPage] = useState(1);
    const [postsPerPage] = useState(5);
    const [refresh, setRefresh] = useState(false);
    const [category,setCategory] = useState();
    const [users,setUsers] = useState();

    useEffect(() => {
        const fetchPosts = async () => {
            setLoading(true);
            const res = await axios.get('https://localhost:5001/api/File/files');
            setPosts(res.data);
            setLoading(false);
        };

        fetchPosts();
    }, [refresh]);
    useEffect(() => {
        const fetchPosts = async () => {
            setLoading(true);
            const res = await axios.get('https://localhost:5001/api/File/files/paging?PageNumber=' + currentPage);
            setFiles(res.data);
            setPage(JSON.parse(res.headers.pagination));
            setLoading(false);
            setRefresh(false);
        };
        fetchPosts();
    }, [currentPage,refresh]);
    useEffect(() => {
        fetch(variables.API_URL + 'Category')
            .then(response => {
                if(!response.ok){
                    throw new Error();
                }
                return response.json()
            })
            .then(data => {
                setCategory(data);
                setRefresh(false);
            }).catch(res => {
                alert("Error");
        })
    }, [refresh])
    useEffect(() => {
        fetch(variables.API_URL + 'Authentication')
            .then(response => {
                if(!response.ok){
                    throw new Error();
                }
                return response.json();
            })
            .then(data => {
                setUsers(data);
                setRefresh(false);
            }).catch(err => {
            alert("Error");
        })
    }, [refresh])
    const refreshPage = () =>{
        setRefresh(true);
    };
    const paginate = pageNumber => setCurrentPage(pageNumber);
    return (
        <>
            <Posts
                posts={files}
                loading={loading}
                refreshPage={refreshPage}
                category={category}
                users={users}
            />
            <Pagination
                postsPerPage={postsPerPage}
                totalPosts={posts.length}
                paginate={paginate}
            />
        </>
    )
};

export default Files;