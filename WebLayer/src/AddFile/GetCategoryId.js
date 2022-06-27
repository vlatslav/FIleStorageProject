const GetCategoryId = (category) =>
{
    if(category === "Games")
        return 1;
    if(category === "Images")
        return 2;
    if(category === "Videos")
        return 3;
    if(category === "Books")
        return 4;
    if(category === "Scripts")
        return 5;
}
export default GetCategoryId;