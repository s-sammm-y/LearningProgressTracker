
document.addEventListener("DOMContentLoaded", () => {

    const searchInput = document.getElementById("activitySearch");

    const clearButton = document.getElementById("clearActivitySearch");

    const activityRows = Array.from(
        document.querySelectorAll(".activity-row")
    );

    const activityCount = document.getElementById("activityCount");

    const noResultsRow = document.getElementById("activityNoResults");

    if (!searchInput || !activityCount || !noResultsRow) {
        return;
    }

    const updateSearch = () => {

        const query = searchInput.value.trim().toLowerCase();

        let visibleCount = 0;

        activityRows.forEach(row => {

            const searchableText =
                (row.dataset.search || "").toLowerCase();

            const matches = searchableText.includes(query);

            row.hidden = !matches;

            if (matches) {
                visibleCount++;
            }

        });

        // Update activity count
        activityCount.textContent =
            `${visibleCount} ${visibleCount === 1 ? "Activity" : "Activities"}`;

        // Display empty search result message
        noResultsRow.hidden = visibleCount !== 0;

        // Show or hide clear button
        clearButton.hidden = query.length === 0;

    };

    // Search as the user types
    searchInput.addEventListener("input", updateSearch);

    // Clear search
    clearButton.addEventListener("click", () => {

        searchInput.value = "";

        updateSearch();

        searchInput.focus();

    });

});
